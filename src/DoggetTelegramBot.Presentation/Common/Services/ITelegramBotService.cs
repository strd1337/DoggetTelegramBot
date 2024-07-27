using DoggetTelegramBot.Presentation.Common.Mapping;
using ErrorOr;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.Common.Services
{
    public interface ITelegramBotService
    {
        Response Problem(List<Error> errors);

        Task<Message> SendMessage(ITelegramBotClient botClient, Update update, string text);
        Task<Message> SendReplyMessage(ITelegramBotClient botClient, Update update, string text);
        Task<Message> EditMessage(ITelegramBotClient botClient, Update update, string text);

        Task<Message> SendMessage(
            ITelegramBotClient botClient,
            Update update,
            string text,
            OptionMessage options);

        Task<Message> EditMessage(
            ITelegramBotClient botClient,
            long chatId,
            int messageId,
            int replyToMessageId,
            string text);

        public Task<Message> EditMessage(
            ITelegramBotClient botClient,
            long chatId,
            int messageId,
            string text);
    }
}
