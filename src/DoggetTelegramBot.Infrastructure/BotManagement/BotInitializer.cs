using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Infrastructure.Configs;
using Microsoft.Extensions.Options;
using PRTelegramBot.Configs;
using PRTelegramBot.Core;
using Telegram.Bot.Polling;

namespace DoggetTelegramBot.Infrastructure.BotManagement
{
    public sealed class BotInitializer(
        IOptions<TelegramBotConfig> options,
        IBotLogger logger,
        BotEventDispatcher dispatcher)
    {
        private readonly TelegramBotConfig config = options.Value;

        public async Task InitializeAndRunAsync(IServiceProvider serviceProvider)
        {
            var bot = new PRBotBuilder(config.Token)
                .SetClearUpdatesOnStart(config.ClearUpdatesOnStart)
                .SetBotId(config.BotId)
                .AddConfigPaths(GetConfigPaths())
                .AddAdmins(config.Admins)
                .AddUsersWhiteList(config.WhiteListUsers)
                .AddRecevingOptions(new ReceiverOptions
                {
                    Limit = 5,
                    AllowedUpdates = [],
                    ThrowPendingUpdates = true,
                })
                .SetServiceProvider(serviceProvider)
                .Build();

            logger.SetBotInstance(bot);

            bot.Events.OnCommonLog += logger.OnLogCommonAsync;
            bot.Events.OnErrorLog += logger.OnLogErrorAsync;
            bot.Events.OnCheckPrivilege += dispatcher.OnCheckPrivilege;

            bot.Events.UpdateEvents.OnPreUpdate += dispatcher.OnCheckUserExistance;

            bot.Events.MessageEvents.OnTextHandle += dispatcher.OnValidMessageReward;
            bot.Events.MessageEvents.OnChatMembersAddedHandle += dispatcher.OnChatMembersAdded;
            bot.Events.MessageEvents.OnChatMemberLeftHandle += dispatcher.OnChatMemberLeft;

            await bot.Start();
        }

        public static Dictionary<string, string> GetConfigPaths()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("commands", ".\\Configs\\commands.json");
            return dictionary;
        }
    }
}
