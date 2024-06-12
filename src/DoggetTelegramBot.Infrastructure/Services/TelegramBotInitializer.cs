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

        public Task<PRBot> InitializeAndRunAsync()
        {
            PRBot telegramBot = new(options =>
            {
                options.Token = config.Token;
                options.ClearUpdatesOnStart = config.ClearUpdatesOnStart;
                options.BotId = config.BotId;
            });

            telegramBot.OnLogCommon += telegramLogger.LogCommon;
            telegramBot.OnLogError += telegramLogger.LogError;

            telegramBot.Start();

            return Task.FromResult(telegramBot);
        }
    }
}
