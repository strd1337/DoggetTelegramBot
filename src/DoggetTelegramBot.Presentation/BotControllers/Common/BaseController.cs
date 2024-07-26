using ErrorOr;
using DoggetTelegramBot.Presentation.Common.Mapping;
using Telegram.Bot.Types;
using PRTelegramBot.Models;
using Telegram.Bot;
using DoggetTelegramBot.Presentation.Common.Services;

namespace DoggetTelegramBot.Presentation.BotControllers.Common
{
    public abstract class BaseController(ITelegramBotService botService)
    {
        protected Response Problem(List<Error> errors)
            => botService.Problem(errors);

        protected Task<Message> SendMessage(ITelegramBotClient botClient, Update update, string text)
            => botService.SendMessage(botClient, update, text);

        protected Task<Message> SendReplyMessage(ITelegramBotClient botClient, Update update, string text)
            => botService.SendReplyMessage(botClient, update, text);

        protected Task<Message> EditMessage(ITelegramBotClient botClient, Update update, string text)
            => botService.EditMessage(botClient, update, text);

        protected Task<Message> SendMessage(
            ITelegramBotClient botClient,
            Update update,
            string text,
            OptionMessage options) => botService.SendMessage(botClient, update, text, options);

        protected Task<Message> EditMessage(
            ITelegramBotClient botClient,
            long chatId,
            int messageId,
            int replyToMessageId,
            string text) => botService.EditMessage(botClient, chatId, messageId, replyToMessageId, text);

        protected Task<Message> EditMessage(
            ITelegramBotClient botClient,
            long chatId,
            int messageId,
            string text) => botService.EditMessage(botClient, chatId, messageId, text);
    }
}
