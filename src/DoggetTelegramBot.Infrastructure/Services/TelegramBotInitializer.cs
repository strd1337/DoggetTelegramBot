using DoggetTelegramBot.Infrastructure.Configs;
using Microsoft.Extensions.Options;
using PRTelegramBot.Core;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public sealed class TelegramBotInitializer(
        IOptions<TelegramBotConfig> options)
    {
        private readonly TelegramBotConfig config = options.Value;

        public Task<PRBot> InitializeAndRunAsync()
        {
            var telegramBot = new PRBot(options =>
            {
                options.Token = config.Token;
                options.ClearUpdatesOnStart = config.ClearUpdatesOnStart;
                options.BotId = config.BotId;
            });

            telegramBot.Start();

            return Task.FromResult(telegramBot);
        }
    }
}
