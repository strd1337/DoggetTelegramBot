using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Infrastructure.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            PRBot telegramBot = new(options =>
            {
                options.Token = config.Token;
                options.ClearUpdatesOnStart = config.ClearUpdatesOnStart;
                options.BotId = config.BotId;
                options.Admins = config.Admins;
                options.WhiteListUsers = config.WhiteListUsers;
            },
            new ReceiverOptions
            {
                Limit = 5,
                AllowedUpdates = [],
                ThrowPendingUpdates = true
            },
            serviceProvider);

            telegramBot.OnLogCommon += logger.LogCommon;
            telegramBot.OnLogError += logger.LogError;

            telegramBot.Handler.OnPreUpdate += dispatcher.OnCheckUserExistance;

            telegramBot.Handler.Router.OnCheckPrivilege += dispatcher.OnCheckPrivilege;

            await telegramBot.Start();
        }
    }
}
