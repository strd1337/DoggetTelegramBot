using DoggetTelegramBot.Infrastructure.BotManagement.Events;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Models.EventsArgs;

namespace DoggetTelegramBot.Infrastructure.BotManagement
{
    public sealed class BotEventDispatcher(
        CommonEventsHandler commonEvents,
        UpdateEventsHandler updateEvents,
        TextEventsHandler textEvents)
    {
        public async Task OnCheckPrivilege(PrivilegeEventArgs args) =>
            await commonEvents.HandleCheckPrivilege(
                args.BotClient,
                args.Update,
                args.ExecuteMethod,
                args.Mask);

        public async Task<UpdateResult> OnCheckUserExistance(BotEventArgs args) =>
            await updateEvents.HandleCheckUserExistance(args.BotClient, args.Update);

        public async Task<UpdateResult> OnChatMembersAdded(BotEventArgs args) =>
            await updateEvents.HandleAddChatMember(args.BotClient, args.Update);

        public async Task<UpdateResult> OnChatMemberLeft(BotEventArgs args) =>
            await updateEvents.HandleChatMemberLeft(args.BotClient, args.Update);

        public async Task<UpdateResult> OnValidMessageReward(BotEventArgs args) =>
            await textEvents.HandleValidMessageReward(args.BotClient, args.Update);
    }
}
