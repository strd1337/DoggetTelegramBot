using DoggetTelegramBot.Infrastructure.BotManagement.Events;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Models.EventsArgs;

namespace DoggetTelegramBot.Infrastructure.BotManagement
{
    public sealed class BotEventDispatcher(
        UserEventsHandler userEvents)
    {
        public async Task OnCheckPrivilege(PrivilegeEventArgs args) =>
            await userEvents.HandleCheckPrivilege(
                args.BotClient,
                args.Update,
                args.ExecuteMethod,
                args.Mask);

        public async Task<UpdateResult> OnCheckUserExistance(BotEventArgs args) =>
            await userEvents.HandleCheckUserExistance(args.Update);
    }
}
