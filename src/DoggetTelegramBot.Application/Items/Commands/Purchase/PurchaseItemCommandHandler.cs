using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Items.Common;
using ErrorOr;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Application.Users.Common;
using MediatR;
using DoggetTelegramBot.Application.Users.Queries.Get;
using DoggetTelegramBot.Domain.Models.ItemEntity;
using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;

namespace DoggetTelegramBot.Application.Items.Commands.Purchase
{
    public sealed class PurchaseItemCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        ICacheService cacheService,
        ITransactionService transactionService,
        IMediator mediator,
        IDateTimeProvider dateTimeProvider) : ICommandHandler<PurchaseItemCommand, PurchaseItemResult>
    {
        public async Task<ErrorOr<PurchaseItemResult>> Handle(
            PurchaseItemCommand request, CancellationToken cancellationToken)
        {
            var userResult = await GetUserByTelegramId(request.TelegramId, cancellationToken);

            if (userResult.IsError)
            {
                return userResult.Errors;
            }

            var itemRepository = unitOfWork.GetRepository<Item, ItemId>();

            var itemResult = await GetItemAsync(
                itemRepository,
                request.Type,
                request.ServerName,
                request.AmountType,
                cancellationToken);

            if (itemResult.IsError)
            {
                return itemResult.Errors;
            }

            var commandUsageResult = await CheckAndSetCommandUsageAsync(
                request.TelegramId,
                nameof(PurchaseItemCommand),
                cancellationToken);

            if (commandUsageResult.IsError)
            {
                return commandUsageResult.Errors;
            }

            var item = itemResult.Value;
            var user = userResult.Value.User;

            decimal amount = item.Price * request.Count;
            var transactionResult = await ExecutePurchaseAsync(
                user,
                amount,
                cancellationToken);

            if (transactionResult.IsError)
            {
                return transactionResult.Errors;
            }

            await DiminishCountAndUpdateItem(itemRepository, item, cancellationToken);

            await RemoveKeysFromCacheAsync(user, cancellationToken);

            return new PurchaseItemResult(item.Value);
        }

        private async Task DiminishCountAndUpdateItem(
            IGenericRepository<Item, ItemId> repository,
            Item item,
            CancellationToken cancellationToken)
        {
            item.DiminishCount();

            if (item.Count == 0)
            {
                item.Delete();
            }

            await repository.UpdateAsync(item);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
                Constants.Item.Messages.Updated(item.ItemId),
                TelegramEvents.Message,
                Constants.LogColors.Update);
        }

        private async Task<ErrorOr<Item>> GetItemAsync(
            IGenericRepository<Item, ItemId> repository,
            ItemType type,
            string serverName,
            ItemAmountType? amountType,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.Item.Messages.GetRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            var item = amountType is not null ?
                await repository.FirstOrDefaultAsync(
                    i => i.Type == type &&
                         i.ServerName == serverName &&
                         i.AmountType == amountType &&
                         !i.IsDeleted,
                    cancellationToken) :
                await repository.FirstOrDefaultAsync(
                    i => i.Type == type &&
                         i.ServerName == serverName &&
                         !i.IsDeleted,
                    cancellationToken);

            if (item is null)
            {
                return Errors.Item.NotFound;
            }

            logger.LogCommon(
                Constants.Item.Messages.GetRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return item;
        }

        private async Task<ErrorOr<bool>> ExecutePurchaseAsync(
            User user,
            decimal amount,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.Transaction.Messages.ExecutePurchase(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            var transactionResult = await transactionService.ExecutePurchaseItemsAsync(
                user.UserId,
                amount,
                cancellationToken);

            logger.LogCommon(
                Constants.Transaction.Messages.ExecutePurchase(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return transactionResult;
        }

        private async Task<ErrorOr<bool>> CheckAndSetCommandUsageAsync(
            long telegramId,
            string commandName,
            CancellationToken cancellationToken)
        {
            string key = CacheKeyGenerator.CommandUsageByTelegramId(telegramId, commandName);

            (bool hasValue, DateTime? cachedTime) = await cacheService
                .TryGetValueAsync<DateTime>(key, cancellationToken);

            if (hasValue)
            {
                var allowedAccessDate = cachedTime.Value.Add(Constants.Item.PurchaseUsageTime);

                return Errors.Permissions.UsageTime(
                    Constants.Item.PurchaseUsageTime,
                    allowedAccessDate);
            }
            Console.WriteLine(dateTimeProvider.UtcNow);
            await cacheService.SetUsageTimeAsync(
                key,
                dateTimeProvider.UtcNow.ToLocalTime(),
                Constants.Item.PurchaseUsageTime,
                cancellationToken);

            return !hasValue;
        }

        private async Task<ErrorOr<GetUserResult>> GetUserByTelegramId(
            long telegramId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.User.Messages.GetRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            GetUserByTelegramIdQuery query = new(telegramId);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                Constants.User.Messages.GetRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return result;
        }

        private async Task RemoveKeysFromCacheAsync(
            User user,
            CancellationToken cancellationToken)
        {
            string[] keys =
            [
                CacheKeyGenerator.GetAllItemServerNames(),
                CacheKeyGenerator.GetUserByTelegramId(user.TelegramId),
            ];

            var removalTasks = keys
                .Select(key => cacheService.RemoveAsync(key, cancellationToken));

            await Task.WhenAll(removalTasks);
        }
    }
}
