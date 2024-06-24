using PRTelegramBot.Core;
using PRTelegramBot.Models.EventsArgs;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Application.Common.Services
{
    public interface IBotLogger
    {
        Task OnLogCommonAsync(CommonLogEventArgs args);
        Task OnLogErrorAsync(ErrorLogEventArgs args);

        void LogCommon(string message, Enum type, ConsoleColor color);
        void LogError(Exception exception, Update? update = null);
        void LogCommon(Update update, ConsoleColor color = ConsoleColor.Blue);

        void SetBotInstance(PRBotBase bot);
    }
}
