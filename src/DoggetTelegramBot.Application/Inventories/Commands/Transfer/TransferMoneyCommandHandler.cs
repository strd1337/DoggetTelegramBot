using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Inventories.Commands.Transfer;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.GetAll.TransactionParticipants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using TransactionConstants = DoggetTelegramBot.Domain.Common.Constants.Transaction.Constants.Transaction;
using InventoryConstants = DoggetTelegramBot.Domain.Common.Constants.Inventory.Constants.Inventory;
using DoggetTelegramBot.Application.Helpers.CacheKeys;

namespace DoggetTelegramBot.Application.Inventories.Commands.Trade
{
    public sealed class TransferMoneyCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        IMediator mediator,
        ITransactionService transactionService,
        ICacheService cacheService) : ICommandHandler<TransferMoneyCommand, TransferMoneyResult>
    {
        public async Task<ErrorOr<TransferMoneyResult>> Handle(
            TransferMoneyCommand request,
            CancellationToken cancellationToken)
        {
            var participantsResult = await GetTransactionParticipantsByTelegramIds(
                request.FromTelegramId,
                request.ToTelegramId,
                cancellationToken);

            if (participantsResult.IsError)
            {
                return participantsResult.Errors;
            }

            var transferResult = await ExecuteTransferAsync(
                participantsResult.Value.FromUser!.UserId,
                participantsResult.Value.ToUser.UserId,
                request.Amount,
                cancellationToken);

            if (transferResult.IsError)
            {
                return transferResult.Errors;
            }

            var userNewBalanceResult = await GetUserNewBalance(
                participantsResult.Value.FromUser.InventoryId,
                cancellationToken);

            if (userNewBalanceResult.IsError)
            {
                return userNewBalanceResult.Errors;
            }

            await RemoveKeysFromCacheAsync(
                request.FromTelegramId,
                request.ToTelegramId,
                cancellationToken);

            return new TransferMoneyResult(request.Amount, userNewBalanceResult.Value);
        }

        private async Task<ErrorOr<GetTransactionParticipantsResult>> GetTransactionParticipantsByTelegramIds(
            long fromTelegramId,
            long toTelegramId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                UserConstants.Requests.GetTransactionParticipants(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetTransactionParticipantsByTelegramIdsQuery query = new(toTelegramId, fromTelegramId);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                UserConstants.Requests.GetTransactionParticipants(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return result;
        }

        private async Task<ErrorOr<bool>> ExecuteTransferAsync(
           UserId fromUserId,
           UserId toUserId,
           decimal amount,
           CancellationToken cancellationToken)
        {
            logger.LogCommon(
                TransactionConstants.Requests.ExecuteTransfer(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            var transferResult = await transactionService.ExecuteTransferFundsAsync(
                fromUserId, toUserId, amount, cancellationToken);

            logger.LogCommon(
                TransactionConstants.Requests.ExecuteTransfer(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return transferResult;
        }

        private async Task<ErrorOr<decimal>> GetUserNewBalance(
            InventoryId fromInventoryId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                InventoryConstants.Requests.GetInformation(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            var inventory = await unitOfWork.GetRepository<Inventory, InventoryId>()
                .FirstOrDefaultAsync(i => i.InventoryId == fromInventoryId, cancellationToken);

            if (inventory is null)
            {
                return Errors.Inventory.NotFound;
            }

            logger.LogCommon(
                InventoryConstants.Requests.GetInformation(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return inventory.YuanBalance;
        }

        private async Task RemoveKeysFromCacheAsync(
            long fromTelegramId,
            long toTelegramId,
            CancellationToken cancellationToken)
        {
            string[] keys =
            [
                InventoryCacheKeyGenerator.GetInventoryInfoByTelegramId(fromTelegramId),
                InventoryCacheKeyGenerator.GetInventoryInfoByTelegramId(toTelegramId),
            ];

            var removalTasks = keys
                .Select(key => cacheService.RemoveAsync(key, cancellationToken));

            await Task.WhenAll(removalTasks);
        }
    }
}
