using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Dungeons.Guimu.Common;
using DoggetTelegramBot.Application.Helpers;
using ErrorOr;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.Get;
using DoggetTelegramBot.Domain.Common.Enums;
using MediatR;
using DoggetTelegramBot.Domain.Models.UserEntity;

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
                Constants.Dungeon.Guimu.StartCommandUsageTime,
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
                    Constants.Dungeon.Guimu.StartCommandUsageTime,
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
                < Constants.Dungeon.Guimu.Rules.ZeroChanceThreshold => (0, false, true),
                < Constants.Dungeon.Guimu.Rules.LoseChanceThreshold =>
                    (random.Next(Constants.Dungeon.Guimu.Rules.MinLossAmount, Constants.Dungeon.Guimu.Rules.MaxLossAmount + 1), false, false),
                < Constants.Dungeon.Guimu.Rules.SpecialCaseChanceThreshold => (Constants.Dungeon.Guimu.Rules.SpecialGainAmount, true, true),
                _ => (random.Next(Constants.Dungeon.Guimu.Rules.MinGainAmount, Constants.Dungeon.Guimu.Rules.MaxGainAmount + 1), false, true)
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
                    Constants.Transaction.Messages.ExecuteRewardUser(),
                    TelegramEvents.Message,
                    Constants.LogColors.Request);

                transactionResult = await transactionService.ExecuteRewardUserAsync(
                    user.UserId,
                    amount,
                    cancellationToken);

                logger.LogCommon(
                    Constants.Transaction.Messages.ExecuteRewardUser(false),
                    TelegramEvents.Message,
                    Constants.LogColors.Request);
            }
            else
            {
                logger.LogCommon(
                   Constants.Transaction.Messages.ExecuteUserPenalty(),
                   TelegramEvents.Message,
                   Constants.LogColors.Request);

                transactionResult = await transactionService.ExecuteUserPenaltyAsync(
                    user.UserId,
                    amount,
                    cancellationToken);

                logger.LogCommon(
                   Constants.Transaction.Messages.ExecuteUserPenalty(false),
                   TelegramEvents.Message,
                   Constants.LogColors.Request);
            }

            return transactionResult;
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
    }
}
