using DoggetTelegramBot.Infrastructure.BotManagement.Events;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace DoggetTelegramBot.Infrastructure.BotManagement
{
    public sealed class BotEventDispatcher(
        UserEventsHandler userEvents)
    {
        public async Task OnCheckPrivilege(
            ITelegramBotClient botclient,
            Update update,
            Func<ITelegramBotClient, Update, Task> callback, int? flags = null) =>
                await userEvents.HandleCheckPrivilege(botclient, update, callback, flags);

        public async Task<ResultUpdate> OnCheckUserExistance(
            ITelegramBotClient botclient,
            Update update) =>
                await userEvents.HandleCheckUserExistance(botclient, update);
    }
}
