using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;
using DoggetTelegramBot.Infrastructure.BotManagement.Common.Handlers;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DoggetTelegramBot.Infrastructure.BotManagement.Events
{
    public sealed class TextEventsHandler(
        IScopeService scopeService,
        IBotLogger logger,
        ICacheService cacheService)
    {
        public async Task<UpdateResult> HandleValidMessageReward(
            ITelegramBotClient botClient,
            Update update)
        {
            if (update?.Message?.Chat.Type is not ChatType.Supergroup)
            {
                return UpdateResult.Continue;
            }

            OptionMessage options = new()
            {
                ReplyToMessageId = update.Message!.MessageId,
            };

            logger.LogCommon(
                Constants.User.Messages.ValidMessageRewardRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            long? telegramId = update.Message?.From?.Id;

            if (telegramId is null)
            {
                await ErrorHandler.HandleMissingInformationError(
                    botClient,
                    update,
                    logger,
                    Constants.LogColors.Problem,
                    options);

                return UpdateResult.Continue;
            }

            long userTelegramId = telegramId.Value;

            string cacheKey = CacheKeyGenerator.UserMessageCountByTelegramId(userTelegramId);
            (bool hasValue, int messageCount) = await cacheService.TryGetValueAsync<int>(cacheKey);

            messageCount = hasValue ? UpdateMessageCount(messageCount) : 0;

            await cacheService.SetSlidingExpirationAsync(
               cacheKey,
               messageCount,
               Constants.User.UserMessageActivityTimeout);

            if (messageCount >= Constants.User.MaxMessageCount)
            {
                decimal amount = Constants.Transaction.Costs.UserMessageReward;
                string successMessage = Constants.User.Messages.RewardSentSuccessfully(
                    amount,
                    RewardType.MessageCount);

                await RewardHandler.RewardUserAsync(
                    botClient,
                    update,
                    scopeService,
                    logger,
                    options,
                    amount,
                    userTelegramId,
                    successMessage);
            }

            logger.LogCommon(
                Constants.User.Messages.ValidMessageRewardRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return UpdateResult.Continue;
        }

        private static int UpdateMessageCount(int messageCount) =>
            messageCount < Constants.User.MaxMessageCount ? ++messageCount : 0;
    }
}
