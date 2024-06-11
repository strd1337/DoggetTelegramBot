using DoggetTelegramBot.Infrastructure.Configs;
using Microsoft.Extensions.Options;
using PRTelegramBot.Core;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

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

            RegisterHandlers(telegramBot);

            return Task.FromResult(telegramBot);
        }

        private static void RegisterHandlers(PRBot telegramBot)
        {
            if (telegramBot.Handler is not null)
            {
                telegramBot.Handler.OnPreUpdate += HandlerOnUpdate;
                telegramBot.Handler.OnPostMessageUpdate += HandlerOnWithoutMessageUpdate;
            }
        }

        private static Task<ResultUpdate> HandlerOnUpdate(
            ITelegramBotClient client, 
            Update update)
        {
            return Task.FromResult(ResultUpdate.Continue);
        }
        
        private static Task HandlerOnWithoutMessageUpdate(
            ITelegramBotClient botClient, 
            Update update)
        {
            return Task.CompletedTask;
        }
    }
}
