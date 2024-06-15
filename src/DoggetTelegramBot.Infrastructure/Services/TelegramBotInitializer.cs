using DoggetTelegramBot.Infrastructure.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PRTelegramBot.Core;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public sealed class TelegramBotInitializer(
        IOptions<TelegramBotConfig> options,
        TelegramLogger telegramLogger)
    {
        private readonly TelegramBotConfig config = options.Value;

        public async Task InitializeAndRunAsync(IServiceProvider serviceProvider)
        {
            PRBot telegramBot = new(options =>
            {
                options.Token = config.Token;
                options.ClearUpdatesOnStart = config.ClearUpdatesOnStart;
                options.BotId = config.BotId;
            },
            serviceProvider);

            telegramBot.OnLogCommon += telegramLogger.LogCommon;
            telegramBot.OnLogError += telegramLogger.LogError;

            await telegramBot.Start();
        }
    }
}
