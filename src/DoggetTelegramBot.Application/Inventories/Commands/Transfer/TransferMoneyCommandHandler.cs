using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Inventories.Commands.Transfer;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.GetAll.TransactionParticipants;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;

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
                Constants.User.Messages.GetTransactionParticipantsRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            GetTransactionParticipantsByTelegramIdsQuery query = new(toTelegramId, fromTelegramId);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                Constants.User.Messages.GetTransactionParticipantsRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return result;
        }

        private async Task<ErrorOr<bool>> ExecuteTransferAsync(
           UserId fromUserId,
           UserId toUserId,
           decimal amount,
           CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.Transaction.Messages.ExecuteTransfer(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            var transferResult = await transactionService.ExecuteTransferFundsAsync(
                fromUserId, toUserId, amount, cancellationToken);

            logger.LogCommon(
                Constants.Transaction.Messages.ExecuteTransfer(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return transferResult;
        }

        private async Task<ErrorOr<decimal>> GetUserNewBalance(
            InventoryId fromInventoryId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.Inventory.Messages.GetInformationRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            var inventory = await unitOfWork.GetRepository<Inventory, InventoryId>()
                .FirstOrDefaultAsync(i => i.InventoryId == fromInventoryId, cancellationToken);

            if (inventory is null)
            {
                return Errors.Inventory.NotFound;
            }

            logger.LogCommon(
                Constants.Inventory.Messages.GetInformationRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return inventory.YuanBalance;
        }

        private async Task RemoveKeysFromCacheAsync(
            long fromTelegramId,
            long toTelegramId,
            CancellationToken cancellationToken)
        {
            string[] keys =
            [
                CacheKeyGenerator.GetInventoryInfoByTelegramId(fromTelegramId),
                CacheKeyGenerator.GetInventoryInfoByTelegramId(toTelegramId),
            ];

            var removalTasks = keys
                .Select(key => cacheService.RemoveAsync(key, cancellationToken));

            await Task.WhenAll(removalTasks);
        }
    }
}
