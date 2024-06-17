using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Constants;
using NLog;
using PRTelegramBot.Extensions;
using System.Text;
using Telegram.Bot.Exceptions;
using System.Globalization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public class TelegramLogger(IDateTimeProvider dateTimeProvider) : ITelegramLogger
    {
        private readonly Dictionary<string, Logger> loggersContainer = [];

        public void LogCommon(
            string message,
            Enum? eventType,
            ConsoleColor color = ConsoleColor.Blue)
        {
            string commonMessage = $"{dateTimeProvider.UtcNow} {message}";
            LogToConsole(commonMessage, color);
            LogToFile(commonMessage, eventType);
        }

        public void LogCommon(
            Update update,
            Enum? eventType,
            ConsoleColor color = ConsoleColor.Blue)
        {
            string commonMessage = CreateLogMessage(update);
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
            LogToConsole(errorMessage, ConsoleColor.Red);
        }

        private static string CreateLogMessage(Update update)
        {
            var message = update.Message;
            var from = message?.From;

            StringBuilder sb = new();
            sb.Append(message?.Date);
            sb.Append(string.Create(CultureInfo.InvariantCulture, $" The user named, "));
            sb.Append(string.Create(CultureInfo.InvariantCulture, $"UserName: {from?.Username ?? "None"}, "));
            sb.Append(string.Create(CultureInfo.InvariantCulture, $"FirstName: {from?.FirstName ?? "None"}, "));
            sb.Append(string.Create(CultureInfo.InvariantCulture, $"LastName: {from?.LastName ?? "None"}, "));

            if (message?.Type == MessageType.Text)
            {
                sb.Append(string.Create(CultureInfo.InvariantCulture, $"sent message {message?.MessageId ?? -1} "));
                sb.Append(string.Create(CultureInfo.InvariantCulture, $"to chat {message?.Chat.Id ?? -1}. "));

                if (message?.ReplyToMessage is not null)
                {
                    sb.Append(string.Create(
                        CultureInfo.InvariantCulture,
                        $"It is a reply to message {message?.ReplyToMessage?.MessageId ?? -1} "));

                    sb.Append(string.Create(
                        CultureInfo.InvariantCulture,
                        $"and has {message?.Entities?.Length ?? -1} message entities. "));
                }

                sb.Append(string.Create(CultureInfo.InvariantCulture, $"Text: {message?.Text ?? "None"}"));
            }
            else
            {
                sb.Append(string.Create(
                    CultureInfo.InvariantCulture,
                    $"sent the command {message?.Text ?? update.Message!.Type.ToString()} "));

                sb.Append(string.Create(CultureInfo.InvariantCulture, $"to chat {message?.Chat.Id ?? -1}. "));
            }

            return sb.ToString();
        }

        private static void LogToConsole(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void LogToFile(string message, Enum? eventType)
        {
            string loggerName = eventType?.GetDescription() ??
                TelegramEvents.All.GetDescription();

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
