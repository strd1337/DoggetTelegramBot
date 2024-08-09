using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Dungeons.Guimu.Common;
using DoggetTelegramBot.Application.Helpers;
using ErrorOr;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.Get;
using DoggetTelegramBot.Domain.Common.Enums;
using MediatR;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DungeonConstants = DoggetTelegramBot.Domain.Common.Constants.Dungeon.Guimu.Constants.Dungeon;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using TransactionConstants = DoggetTelegramBot.Domain.Common.Constants.Transaction.Constants.Transaction;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;

namespace DoggetTelegramBot.Application.Dungeons.Guimu.Commands
{
    public sealed class GuimuDungeonCommandHandler(
        ITransactionService transactionService,
        IBotLogger logger,
        IMediator mediator,
        CommandUsageManager commandUsage) : ICommandHandler<GuimuDungeonCommand, GuimuDungeonResult>
    {
        public async Task<ErrorOr<GuimuDungeonResult>> Handle(
            GuimuDungeonCommand request,
            CancellationToken cancellationToken)
        {
            var commandUsageResult = await commandUsage.CheckCommandUsageTimeAsync(
                request.ParticipantTelegramId,
                nameof(GuimuDungeonCommand),
                DungeonConstants.Guimu.StartCommandUsageTime,
                cancellationToken);

            if (commandUsageResult.IsError)
            {
                return commandUsageResult.Errors;
            }

            var userResult = await GetUserByTelegramId(request.ParticipantTelegramId, cancellationToken);

            if (userResult.IsError)
            {
                return userResult.Errors;
            }

            var user = userResult.Value.User;
            var (amount, isSpecialCase, isPositive) = GenerateResult();

            var transactionResult = await ExecuteDungeonResultAsync(
                user,
                amount,
                isPositive,
                cancellationToken);

            if (transactionResult.IsError)
            {
                return transactionResult.Errors;
            }

            if (!isSpecialCase)
            {
                await commandUsage.SetCommandUsageTimeAsync(
                    request.ParticipantTelegramId,
                    nameof(GuimuDungeonCommand),
                    DungeonConstants.Guimu.StartCommandUsageTime,
                    cancellationToken);
            }

            return new GuimuDungeonResult(amount, isSpecialCase, isPositive);
        }

        private static (int Amount, bool IsSpecialCase, bool IsPositive) GenerateResult()
        {
            Random random = new();

            int chance = random.Next(0, 100);

            return chance switch
            {
                < DungeonConstants.Guimu.Rules.ZeroChanceThreshold => (0, false, true),
                < DungeonConstants.Guimu.Rules.LoseChanceThreshold =>
                    (random.Next(DungeonConstants.Guimu.Rules.MinLossAmount, DungeonConstants.Guimu.Rules.MaxLossAmount + 1), false, false),
                < DungeonConstants.Guimu.Rules.SpecialCaseChanceThreshold => (DungeonConstants.Guimu.Rules.SpecialGainAmount, true, true),
                _ => (random.Next(DungeonConstants.Guimu.Rules.MinGainAmount, DungeonConstants.Guimu.Rules.MaxGainAmount + 1), false, true)
            };
        }

        private async Task<ErrorOr<bool>> ExecuteDungeonResultAsync(
            User user,
            decimal amount,
            bool isPositive,
            CancellationToken cancellationToken)
        {
            ErrorOr<bool> transactionResult;
            if (isPositive)
            {
                logger.LogCommon(
                    TransactionConstants.Requests.ExecuteRewardUser(),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.Request);

                transactionResult = await transactionService.ExecuteRewardUserAsync(
                    user.UserId,
                    amount,
                    cancellationToken);

                logger.LogCommon(
                    TransactionConstants.Requests.ExecuteRewardUser(false),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.Request);
            }
            else
            {
                logger.LogCommon(
                   TransactionConstants.Requests.ExecuteUserPenalty(),
                   TelegramEvents.Message,
                   LoggerConstants.Colors.Request);

                transactionResult = await transactionService.ExecuteUserPenaltyAsync(
                    user.UserId,
                    amount,
                    cancellationToken);

                logger.LogCommon(
                   TransactionConstants.Requests.ExecuteUserPenalty(false),
                   TelegramEvents.Message,
                   LoggerConstants.Colors.Request);
            }

            return transactionResult;
        }

        private async Task<ErrorOr<GetUserResult>> GetUserByTelegramId(
            long telegramId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                UserConstants.Requests.Get(),
            TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetUserByTelegramIdQuery query = new(telegramId);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                UserConstants.Requests.Get(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return result;
        }
    }
}
