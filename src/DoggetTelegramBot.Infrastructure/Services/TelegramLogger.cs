﻿using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Constants;
using NLog;
using PRTelegramBot.Extensions;
using System.Text;
using Telegram.Bot.Exceptions;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public class TelegramLogger(IDateTimeProvider dateTimeProvider)
    {
        private readonly Dictionary<string, Logger> LoggersContainer = [];

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

            if (!LoggersContainer.TryGetValue(loggerName, out var logger))
            {
                logger = LogManager.GetLogger(loggerName);
                LoggersContainer[loggerName] = logger;
            }

            logger.Info(message);
        }

        private string HandleApiRequestException(ApiRequestException apiEx, long? id)
        {
            StringBuilder errorMessage = new();

            errorMessage.Append($"{dateTimeProvider.UtcNow} {apiEx.Message}. ");
            errorMessage.Append($"Error Code: {apiEx.ErrorCode}. Error Type: {apiEx.GetType().Name}. ");

            if (apiEx.Message.Contains(Constants.ErrorMessage.BotBlockedByUser) ||
                apiEx.Message.Contains(Constants.ErrorMessage.PrivacyRestricted))
            {
                errorMessage.Append($"User {id.GetValueOrDefault()} has blocked the bot.");
            }
            else if (apiEx.Message.Contains(Constants.ErrorMessage.GroupChatUpgraded))
            {
                errorMessage.Append($"\nNew chat id: {apiEx?.Parameters?.MigrateToChatId.GetValueOrDefault()}");
            }

            return errorMessage.ToString();
        }

        private void LogErrorMessage(string errorMessage)
        {
            if (!LoggersContainer.TryGetValue(Constants.Logger.ErrorName, out var logger))
            {
                logger = LogManager.GetLogger(Constants.Logger.ErrorName);
                LoggersContainer[Constants.Logger.ErrorName] = logger;
            }
            
            logger.Error(errorMessage);
        }
    }
}
