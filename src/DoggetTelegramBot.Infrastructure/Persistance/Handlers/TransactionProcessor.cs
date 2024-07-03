using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.TransactionEntity;
using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Infrastructure.Persistance.Processors
{
    public sealed class TransactionProcessor(
        IGenericRepository<Inventory, InventoryId> inventoryRepository,
        IGenericRepository<Transaction, TransactionId> transactionRepository,
        IGenericRepository<User, UserId> userRepository)
    {
        public async Task<ErrorOr<bool>> ProcessTransactionAsync(
            Transaction transaction,
            CancellationToken cancellationToken) =>
                transaction.Type is TransactionType.Purchase ?
                    await ProcessPurchaseTransactionAsync(transaction, cancellationToken) :
                    await ProcessOtherTransactionAsync(transaction, cancellationToken);

        private async Task<ErrorOr<bool>> ProcessPurchaseTransactionAsync(
            Transaction transaction,
            CancellationToken cancellationToken)
        {
            var userId = transaction.ToUserIds.First();
            var user = await userRepository.FirstOrDefaultAsync(
                u => u.UserId.Value == userId.Value,
                cancellationToken);

            if (user is null)
            {
                return Errors.User.NotFound;
            }

            var inventory = await inventoryRepository
                .FirstOrDefaultAsync(i => i.InventoryId == user.InventoryId, cancellationToken);

            if (inventory is null)
            {
                return Errors.Inventory.NotFound;
            }

            bool isValid = CheckInventoryValidity(transaction, inventory);
            if (!isValid)
            {
                return Errors.Inventory.InsufficientBalance;
            }

            inventory!.DeductBalance(transaction.Amount!.Value);
            inventory.AddItems([.. transaction.ItemIds]);

            await inventoryRepository.UpdateAsync(inventory);
            await transactionRepository.AddAsync(transaction, cancellationToken);

            return true;
        }

        private async Task<ErrorOr<bool>> ProcessOtherTransactionAsync(
            Transaction transaction,
            CancellationToken cancellationToken)
        {
            var fromUsers = GetUsers(transaction.FromUserIds);
            var toUsers = GetUsers(transaction.ToUserIds);

            if (fromUsers.Count() != transaction.FromUserIds.Count ||
                toUsers.Count() != transaction.ToUserIds.Count)
            {
                return Errors.User.SomeNotFound;
            }

            var fromInventories = GetInventories(fromUsers);
            var toInventories = GetInventories(toUsers);

            if (fromInventories.Count() != fromUsers.Count() ||
                toInventories.Count() != toUsers.Count())
            {
                return Errors.Inventory.SomeNotFound;
            }

            var validationResult = await ValidateAndUpdateFromInventoriesAsync(
                transaction,
                [.. fromInventories],
                cancellationToken);

            if (validationResult.IsError)
            {
                return validationResult;
            }

            await UpdateToInventories(transaction, [.. toInventories]);

            await transactionRepository.AddAsync(transaction, cancellationToken);

            return true;
        }

        private IQueryable<User> GetUsers(IEnumerable<UserId> userIds) =>
            userRepository.GetWhere(u => userIds.Contains(u.UserId) && !u.IsDeleted);

        private IQueryable<Inventory> GetInventories(IQueryable<User> users) =>
            inventoryRepository.GetWhere(
                i => users.Select(u => u.InventoryId).Contains(i.InventoryId) && !i.IsDeleted);

        private async Task<ErrorOr<bool>> ValidateAndUpdateFromInventoriesAsync(
            Transaction transaction,
            List<Inventory> fromInventories,
            CancellationToken cancellationToken = default)
        {
            foreach (var inventory in fromInventories)
            {
                bool isValid = CheckInventoryValidity(transaction, inventory);
                if (!isValid)
                {
                    return Errors.Inventory.InsufficientInventories;
                }

                if (transaction.Amount is not null)
                {
                    inventory.DeductBalance(transaction.Amount.Value);
                }

                if (transaction.ItemIds.Count > 0)
                {
                    inventory.DeductItems([.. transaction.ItemIds]);
                }

                await inventoryRepository.UpdateAsync(inventory);
            }

            return true;
        }

        private async Task UpdateToInventories(
            Transaction transaction,
            List<Inventory> toInventories)
        {
            foreach (var inventory in toInventories)
            {
                if (transaction.Amount is not null)
                {
                    inventory.IncreaseBalance(transaction.Amount.Value);
                }

                if (transaction.ItemIds.Count > 0)
                {
                    inventory.AddItems([.. transaction.ItemIds]);
                }

                await inventoryRepository.UpdateAsync(inventory);
            }
        }

        private static bool CheckInventoryValidity(
            Transaction transaction,
            Inventory inventory)
        {
            decimal? amount = transaction.Amount;
            var itemIds = transaction.ItemIds;

            bool hasSufficientBalance = amount is null ||
                inventory.HasSufficientBalance(amount.Value);

            bool hasSufficientItems = itemIds.Count == 0 ||
                inventory.HasSufficientItems([.. itemIds]);

            return transaction.Type switch
            {
                TransactionType.ServiceFee => amount is not null && hasSufficientBalance,
                TransactionType.Purchase => hasSufficientBalance,
                _ => hasSufficientBalance && hasSufficientItems
            };
        }
    }
}
