using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Infrastructure.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PRTelegramBot.Core;
using Telegram.Bot;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public sealed class TelegramBotInitializer(
        IOptions<TelegramBotConfig> options,
        ITelegramLogger logger)
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

            telegramBot.OnLogError += logger.LogError;

            await telegramBot.Start();

            string? botName = (await telegramBot.botClient.GetMeAsync())?.Username;
            logger.LogCommon(
                $"Bot {botName} is running.",
                TelegramEvents.Initialization,
                ConsoleColor.Green);
        }
    }
}
