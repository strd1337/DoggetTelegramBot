using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using ErrorOr;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Helpers = PRTelegramBot.Helpers;

namespace DoggetTelegramBot.Infrastructure.BotManagement.Common.Handlers
{
    public static class ErrorHandler
    {
        public static async Task HandleMissingInformationError(
            ITelegramBotClient botClient,
            Update update,
            IBotLogger logger,
            ConsoleColor consoleColor,
            OptionMessage options)
        {
            LogError(Constants.ErrorMessage.MissingInformation, logger, consoleColor);
            await Helpers.Message.Send(
                botClient,
                update,
                Constants.ErrorMessage.MissingInformation,
                options);
        }

        public static async Task HandleTransactionError(
            ITelegramBotClient botClient,
            Update update,
            OptionMessage options,
            Error error,
            IBotLogger logger,
            ConsoleColor consoleColor)
        {
            string errorMessage = $"{error.Code}: {error.Description}";
            LogError(errorMessage, logger, consoleColor);

            await Helpers.Message.Send(
                botClient,
                update,
                error.Description,
                options);
        }

        private static void LogError(string message, IBotLogger logger, ConsoleColor consoleColor) =>
            logger.LogCommon(message, TelegramEvents.Message, consoleColor);
    }
}
