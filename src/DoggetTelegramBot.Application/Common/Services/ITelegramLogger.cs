using Telegram.Bot.Types;

namespace DoggetTelegramBot.Application.Common.Services
{
    public interface ITelegramLogger
    {
        void LogCommon(
            string message,
            Enum? eventType,
            ConsoleColor color = ConsoleColor.Blue);

        void LogError(Exception ex, long? id = null);

        void LogCommon(
            Update update,
            Enum? eventType,
            ConsoleColor color = ConsoleColor.Blue);
    }
}
