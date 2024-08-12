using DoggetTelegramBot.Domain.Common.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.Helpers.Common
{
    public static class CallbackQueryHelper
    {
        public static async Task<bool> IsUserAllowedAsync(ITelegramBotClient botClient, Update update)
        {
            long currentUser = update.CallbackQuery!.From!.Id;
            long necessaryUser = update.CallbackQuery.Message!.ReplyToMessage!.From!.Id;

            if (currentUser != necessaryUser)
            {
                await botClient.AnswerCallbackQueryAsync(
                    update.CallbackQuery.Id,
                    Constants.Messages.NotAllowed);

                return false;
            }

            return true;
        }
    }
}
