using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Transactions.Common;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.GetAll;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using TransactionConstants = DoggetTelegramBot.Domain.Common.Constants.Transaction.Constants.Transaction;

namespace DoggetTelegramBot.Application.Transactions.Commands
{
    public sealed class RewardTransactionCommandHandler(
        ITransactionService transactionService,
        IBotLogger logger,
        IMediator mediator) : ICommandHandler<RewardTransactionCommand, RewardTransactionResult>
    {
        public async Task<ErrorOr<RewardTransactionResult>> Handle(
            RewardTransactionCommand request,
            CancellationToken cancellationToken)
        {
            var usersResult = await GetAllUsersByTelegramIds(
                request.UserTelegramIds,
                cancellationToken);

            if (usersResult.IsError)
            {
                return usersResult.Errors;
            }

            var users = usersResult.Value.Users;

            var tasks = users
                .Select(user => ExecuteRewardUserAsync(user.UserId, request.Amount, cancellationToken))
                .ToArray();

            var results = await Task.WhenAll(tasks);

            var errors = results
                .Where(r => r.IsError)
                .SelectMany(r => r.Errors)
                .ToArray();

            return errors.Length != 0 ? errors : new RewardTransactionResult();
        }

        private async Task<ErrorOr<bool>> ExecuteRewardUserAsync(
           UserId userId,
           decimal amount,
           CancellationToken cancellationToken)
        {
            logger.LogCommon(
                TransactionConstants.Requests.ExecuteRewardUser(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            var transactionResult = await transactionService.ExecuteRewardUserAsync(
                userId,
                amount,
                cancellationToken);

            logger.LogCommon(
                TransactionConstants.Requests.ExecuteRewardUser(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return transactionResult;
        }

        private async Task<ErrorOr<GetAllUsersResult>> GetAllUsersByTelegramIds(
            List<long> telegramIds,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                UserConstants.Requests.Get(),
            TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetAllUsersByTelegramIdsQuery query = new(telegramIds);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                UserConstants.Requests.Get(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return result;
        }
    }
}
