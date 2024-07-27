using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.Common.Mapping;
using ErrorOr;
using PRTelegramBot.Models;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.Common.Services
{
    public class TelegramBotService(IBotLogger logger) : ITelegramBotService
    {
        public async Task<Message> SendMessage(
            ITelegramBotClient botClient,
            Update update,
            string text) => await PRTelegramBot.Helpers.Message.Send(botClient, update, text);

        public async Task<Message> SendReplyMessage(
            ITelegramBotClient botClient,
            Update update,
            string text)
        {
            OptionMessage options = new()
            {
                ReplyToMessageId = update.Message!.MessageId,
            };

            return await PRTelegramBot.Helpers.Message.Send(botClient, update, text, options);
        }

        public async Task<Message> SendMessage(
            ITelegramBotClient botClient,
            Update update,
            string text,
            OptionMessage options) => await PRTelegramBot.Helpers.Message.Send(botClient, update, text, options);

        public async Task<Message> EditMessage(
            ITelegramBotClient botClient,
            Update update,
            string text) => await PRTelegramBot.Helpers.Message.Edit(botClient, update, text);

        public async Task<Message> EditMessage(
            ITelegramBotClient botClient,
            long chatId,
            int messageId,
            int replyToMessageId,
            string text)
        {
            OptionMessage options = new()
            {
                ReplyToMessageId = replyToMessageId
            };

            return await PRTelegramBot.Helpers.Message.Edit(botClient, chatId, messageId, text, options);
        }

        public async Task<Message> EditMessage(
            ITelegramBotClient botClient,
            long chatId,
            int messageId,
            string text) => await PRTelegramBot.Helpers.Message.Edit(botClient, chatId, messageId, text);

        public Response Problem(List<Error> errors)
        {
            if (errors.Count == 0)
            {
                logger.LogCommon(
                    "No errors provided",
                    TelegramEvents.Message,
                    Constants.LogColors.Problem);

                return new Response();
            }

            return errors.All(error => error.Type == ErrorType.Validation) ?
                ValidationProblem(errors) :
                Problem(errors[0]);
        }

        private Response ValidationProblem(List<Error> errors)
        {
            StringBuilder validationErrorMessage = new();
            StringBuilder validationErrorDetails = new(validationErrorMessage.ToString());

            foreach (var error in errors)
            {
                validationErrorMessage.AppendLine($" {error.Description}");
                validationErrorDetails.AppendLine($" Error Code: {error.Code}, Description: {error.Description}");
            }

            logger.LogCommon(
                validationErrorDetails.ToString(),
                TelegramEvents.Message,
                Constants.LogColors.Problem);

            return new Response { Message = validationErrorMessage.ToString() };
        }

        private Response Problem(Error error)
        {
            int statusCode = GetStatusCode(error.Type);

            string errorMessage = statusCode switch
            {
                StatusCodes.Status409Conflict => string.Format(null, CompositeFormat.Parse(Constants.ErrorMessage.Conflict), error.Description),
                StatusCodes.Status400BadRequest => string.Format(null, CompositeFormat.Parse(Constants.ErrorMessage.BadRequest), error.Description),
                StatusCodes.Status404NotFound => error.Description,
                StatusCodes.Status401Unauthorized => Constants.ErrorMessage.Unauthorized,
                StatusCodes.Status403Forbidden => error.Description,
                StatusCodes.Status500InternalServerError => Constants.ErrorMessage.InternalServer,
                _ => Constants.ErrorMessage.Generic,
            };

            logger.LogCommon(
                $" Error Code: {error.Code}, Description: {error.Description}",
                TelegramEvents.Message,
                Constants.LogColors.Problem);

            return new Response { Message = errorMessage };
        }

        private static int GetStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };
    }
}
