using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.TransactionEntity;
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
            CancellationToken cancellationToken)
        {
            var fromUsers = GetUsers(transaction.FromUserIds);

            if (fromUsers.Count() != transaction.FromUserIds.Count)
            {
                return Errors.User.SomeNotFound;
            }

            var fromInventories = GetInventories(fromUsers);

            if (fromInventories.Count() != fromUsers.Count())
            {
                return Errors.Inventory.SomeNotFound;
            }

            var validationResult = await ValidateAndUpdateFromInventoriesAsync(
                transaction,
                [.. fromInventories]);

            if (validationResult.IsError)
            {
                return validationResult;
            }

            if (transaction.ToUserId is not null)
            {
                var findAndUpdateResult = await FindAndUpdateToInventory(transaction, cancellationToken);

                if (findAndUpdateResult.IsError)
                {
                    return findAndUpdateResult;
                }
            }

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
            List<Inventory> fromInventories)
        {
            foreach (var inventory in fromInventories)
            {
                var result = await CheckAndUpdateInventory(inventory, transaction);

                if (result.IsError)
                {
                    return fromInventories.Count > 1 ?
                        Errors.Inventory.InsufficientBalances :
                        Errors.Inventory.InsufficientBalance;
                }
            }

            return true;
        }

        private async Task<ErrorOr<bool>> FindAndUpdateToInventory(
            Transaction transaction,
            CancellationToken cancellationToken,
            bool isFromUser = false)
        {
            var inventoryResult = await GetInventory(transaction, cancellationToken);

            if (inventoryResult.IsError)
            {
                return inventoryResult.Errors;
            }

            var checkAndUpdateResult = await CheckAndUpdateInventory(
                inventoryResult.Value,
                transaction,
                isFromUser);

            return checkAndUpdateResult.IsError ?
                checkAndUpdateResult.Errors :
                true;
        }

        private async Task<ErrorOr<Inventory>> GetInventory(
            Transaction transaction,
            CancellationToken cancellationToken)
        {
            var user = await userRepository.FirstOrDefaultAsync(
                u => u.UserId == transaction.ToUserId,
                cancellationToken);

            if (user is null)
            {
                return Errors.User.NotFound;
            }

            var inventory = await inventoryRepository
                .FirstOrDefaultAsync(i => i.InventoryId == user.InventoryId, cancellationToken);

            return inventory is null ?
                Errors.Inventory.NotFound :
                inventory;
        }

        private async Task<ErrorOr<bool>> CheckAndUpdateInventory(
            Inventory inventory,
            Transaction transaction,
            bool isFromUser = true)
        {
            if (!inventory.HasSufficientBalance(transaction.Amount))
            {
                return Errors.Inventory.InsufficientBalance;
            }

            if (isFromUser)
            {
                inventory.DeductBalance(transaction.Amount);
            }
            else
            {
                inventory.IncreaseBalance(transaction.Amount);
            }

            await inventoryRepository.UpdateAsync(inventory);

            return true;
        }
    }
}
