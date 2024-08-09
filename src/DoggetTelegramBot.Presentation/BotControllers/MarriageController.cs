using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.BotCommands;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Services;
using DoggetTelegramBot.Presentation.Handlers.Requests;
using DoggetTelegramBot.Presentation.Helpers.MenuGenerators;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using MarriageConstants = DoggetTelegramBot.Domain.Common.Constants.Marriage.Constants.Marriage;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public class MarriageController(
        IBotLogger logger,
        MarriageRequestHandler requestHandler,
        ITelegramBotService botService) : BaseController(botService)
    {
        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.Marriage.Marry)]
        public async Task MarryAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                MarriageConstants.Requests.Marry(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            if (update.Message?.ReplyToMessage is null)
            {
                await SendReplyMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Commands.Marriage.Marry));

                logger.LogCommon(
                    MarriageConstants.Requests.Marry(false),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.Request);

                return;
            }

            var menu = MarriageMenuGenerator.GenerateConfirmationMenu(update);

            var message = await SendMessage(
                botClient,
                update,
                MarriageConstants.Messages.ComposeMarryOrDivorceProposal(
                    update.Message.From!.FirstName,
                    update.Message.From!.Username,
                    update.Message.ReplyToMessage.From!.FirstName,
                    update.Message.ReplyToMessage.From.Username),
                menu);

            _ = requestHandler.HandleGetMarriedAsync(botClient, update, message);
        }

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.Marriage.Divorce)]
        public async Task DivorceAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                MarriageConstants.Requests.Divorce(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            if (update.Message?.ReplyToMessage is null)
            {
                await SendReplyMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Commands.Marriage.Divorce));

                logger.LogCommon(
                    MarriageConstants.Requests.Divorce(false),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.Request);

                return;
            }

            var menu = MarriageMenuGenerator.GenerateConfirmationMenu(update);

            var message = await SendMessage(
                botClient,
                update,
                MarriageConstants.Messages.ComposeMarryOrDivorceProposal(
                    update.Message.From!.FirstName,
                    update.Message.From!.Username,
                    update.Message.ReplyToMessage.From!.FirstName,
                    update.Message.ReplyToMessage.From.Username,
                    false),
                menu);

            _ = requestHandler.HandleDivorceAsync(botClient, update, message);
        }
    }
}
