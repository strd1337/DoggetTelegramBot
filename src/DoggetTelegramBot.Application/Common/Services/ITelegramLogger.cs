namespace DoggetTelegramBot.Application.Common.Services
{
    public interface ITelegramLogger
    {
        void LogCommon(
            string message,
            Enum? eventType,
            ConsoleColor color = ConsoleColor.Blue);

        void LogError(Exception ex, long? id = null);

    }
}
