using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Constants;
using NLog;
using PRTelegramBot.Extensions;
using System.Text;
using Telegram.Bot.Exceptions;
using System.Globalization;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public class TelegramLogger(IDateTimeProvider dateTimeProvider) : ITelegramLogger
    {
        private readonly Dictionary<string, Logger> loggersContainer = [];

        public void LogCommon(string message, Enum? eventType, ConsoleColor color = ConsoleColor.Blue)
        {
            string commonMessage = $"{dateTimeProvider.UtcNow} {message}";
            LogToConsole(commonMessage, color);
            LogToFile(commonMessage, eventType);
        }

        public void LogError(Exception ex, long? id = null)
        {
            string errorMessage = $"{dateTimeProvider.UtcNow} : {ex}";

            if (ex is ApiRequestException apiEx)
            {
                errorMessage = HandleApiRequestException(apiEx, id);
            }

            LogErrorMessage(errorMessage);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
        }

        private static void LogToConsole(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void LogToFile(string message, Enum? eventType)
        {
            string loggerName = eventType?.GetDescription() ?? TelegramEvents.None.GetDescription();

            if (!loggersContainer.TryGetValue(loggerName, out var logger))
            {
                logger = LogManager.GetLogger(loggerName);
                loggersContainer[loggerName] = logger;
            }

            logger.Info(message);
        }

        private string HandleApiRequestException(ApiRequestException apiEx, long? id)
        {
            StringBuilder errorMessage = new();

            errorMessage.Append(
                string.Create(CultureInfo.InvariantCulture,
                $"{dateTimeProvider.UtcNow} {apiEx.Message}. "));

            errorMessage.Append(
                string.Create(CultureInfo.InvariantCulture,
                $"Error Code: {apiEx.ErrorCode}. Error Type: {apiEx.GetType().Name}. "));

            if (apiEx.Message.Contains(Constants.ErrorMessage.BotBlockedByUser) ||
                apiEx.Message.Contains(Constants.ErrorMessage.PrivacyRestricted))
            {
                errorMessage.Append(
                    string.Create(CultureInfo.InvariantCulture,
                    $"User {id.GetValueOrDefault()} has blocked the bot."));
            }
            else if (apiEx.Message.Contains(Constants.ErrorMessage.GroupChatUpgraded))
            {
                errorMessage.Append(
                    string.Create(CultureInfo.InvariantCulture,
                    $"\nNew chat id: {apiEx?.Parameters?.MigrateToChatId.GetValueOrDefault()}"));
            }

            return errorMessage.ToString();
        }

        private void LogErrorMessage(string errorMessage)
        {
            if (!loggersContainer.TryGetValue(Constants.Logger.ErrorName, out var logger))
            {
                logger = LogManager.GetLogger(Constants.Logger.ErrorName);
                loggersContainer[Constants.Logger.ErrorName] = logger;
            }

            logger.Error(errorMessage);
        }
    }
}
