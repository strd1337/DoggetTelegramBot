using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;
using DoggetTelegramBot.Infrastructure.BotManagement.Common.Handlers;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using TransactionConstants = DoggetTelegramBot.Domain.Common.Constants.Transaction.Constants.Transaction;
using DoggetTelegramBot.Application.Helpers.CacheKeys;

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
            if (update?.Message?.Chat.Type is not ChatType.Supergroup ||
                update?.Message.Type is not MessageType.Text)
            {
                return UpdateResult.Continue;
            }

            OptionMessage options = new()
            {
                ReplyToMessageId = update.Message!.MessageId,
            };

            logger.LogCommon(
                UserConstants.Requests.ValidMessageReward(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            long? telegramId = update.Message?.From?.Id;

            if (telegramId is null)
            {
                await ErrorHandler.HandleMissingInformationError(
                    botClient,
                    update,
                    logger,
                    LoggerConstants.Colors.Problem,
                    options);

                return UpdateResult.Continue;
            }

            long userTelegramId = telegramId.Value;

            string cacheKey = UserCacheKeyGenerator.UserMessageCountByTelegramId(userTelegramId);
            (bool hasValue, int messageCount) = await cacheService.TryGetValueAsync<int>(cacheKey);

            messageCount = hasValue ? UpdateMessageCount(messageCount) : 0;

            await cacheService.SetSlidingExpirationAsync(
               cacheKey,
               messageCount,
               UserConstants.UserMessageActivityTimeout);

            if (messageCount >= UserConstants.MaxMessageCount)
            {
                decimal amount = TransactionConstants.Costs.UserMessageReward;
                string successMessage = UserConstants.Messages.RewardSentSuccessfully(
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
                UserConstants.Requests.ValidMessageReward(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return UpdateResult.Continue;
        }

        private static int UpdateMessageCount(int messageCount) =>
            messageCount < UserConstants.MaxMessageCount ? ++messageCount : 0;
    }
}
