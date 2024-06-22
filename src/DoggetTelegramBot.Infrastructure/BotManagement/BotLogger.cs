using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;
using NLog;
using System.Text;
using Telegram.Bot.Exceptions;
using System.Globalization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using PRTelegramBot.Models.EventsArgs;
using PRTelegramBot.Core;
using PRTelegramBot.Extensions;

namespace DoggetTelegramBot.Infrastructure.BotManagement
{
    public class BotLogger(IDateTimeProvider dateTimeProvider) : IBotLogger
    {
        private readonly Dictionary<string, Logger> loggersContainer = [];
        private PRBotBase bot = null!;

        public void SetBotInstance(PRBotBase bot) => this.bot = bot;

        public void LogCommon(string message, Enum type, ConsoleColor color)
        {
            CommonLogEventArgsCreator argsCreator = new(
                message,
                type.GetDescription(),
                color);

            CommonLogEventArgs eventArgs = new(bot, argsCreator);
            OnLogCommonAsync(eventArgs);
        }

        public void LogCommon(Update update, ConsoleColor color = ConsoleColor.Blue)
        {
            CommonLogEventArgsCreator argsCreator = new(
                string.Empty,
                string.Empty,
                color,
                update);

            CommonLogEventArgs eventArgs = new(bot, argsCreator);
            OnLogCommonAsync(eventArgs);
        }

        public void LogError(Exception exception, Update? update = null)
        {
            ErrorLogEventArgsCreator argsCreator = update is null ?
                new(exception) :
                new(exception, update);

            ErrorLogEventArgs eventArgs = new(bot, argsCreator);
            OnLogErrorAsync(eventArgs);
        }

        public Task OnLogCommonAsync(CommonLogEventArgs args)
        {
            string commonMessage = args.Update.Message is null ?
                $"{dateTimeProvider.UtcNow} {args.Message}" :
                CreateLogMessage(args.Update);

            LogToConsole(commonMessage, args.Color);
            Task.Run(() => LogToFileAsync(commonMessage, args.Type));
            return Task.CompletedTask;
        }

        public Task OnLogErrorAsync(ErrorLogEventArgs args)
        {
            string errorMessage = $"{dateTimeProvider.UtcNow} : {args.Exception}";

            if (args.Exception is ApiRequestException apiEx)
            {
                errorMessage = HandleApiRequestException(apiEx, args.Update.Message!.From!.Id);
            }

            Task.Run(() => LogErrorMessageAsync(errorMessage));
            LogToConsole(errorMessage, ConsoleColor.Red);

            return Task.CompletedTask;
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

        private async Task LogToFileAsync(string message, string eventType)
        {
            if (!loggersContainer.TryGetValue(eventType, out var logger))
            {
                logger = LogManager.GetLogger(eventType);
                loggersContainer[eventType] = logger;
            }

            await Task.Run(() => logger.Info(message));
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

        private async Task LogErrorMessageAsync(string errorMessage)
        {
            if (!loggersContainer.TryGetValue(Constants.Logger.ErrorName, out var logger))
            {
                logger = LogManager.GetLogger(Constants.Logger.ErrorName);
                loggersContainer[Constants.Logger.ErrorName] = logger;
            }

            await Task.Run(() => logger.Error(errorMessage));
        }
    }
}
