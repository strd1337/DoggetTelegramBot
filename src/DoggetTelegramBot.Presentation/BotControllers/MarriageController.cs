using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Services;
using DoggetTelegramBot.Presentation.Handlers.Requests;
using DoggetTelegramBot.Presentation.Helpers.MenuGenerators;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public class MarriageController(
        IBotLogger logger,
        MarriageRequestHandler requestHandler,
        ITelegramBotService botService) : BaseController(botService)
    {
        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.Marriage.ReplyKeys.Marry)]
        public async Task Marry(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Marriage.Messages.MarryRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            if (update.Message?.ReplyToMessage is null)
            {
                await SendReplyMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Constants.Marriage.ReplyKeys.Marry));

                return;
            }

            var menu = MarriageMenuGenerator.GenerateConfirmationMenu(update);

            var message = await SendMessage(
                botClient,
                update,
                Constants.Marriage.Messages.ComposeMarryOrDivorceProposal(
                    update.Message.From!.FirstName,
                    update.Message.From!.Username,
                    update.Message.ReplyToMessage.From!.FirstName,
                    update.Message.ReplyToMessage.From.Username),
                menu);

            _ = requestHandler.HandleGetMarriedAsync(botClient, update, message);
        }

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.Marriage.ReplyKeys.Divorce)]
        public async Task Divorce(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Marriage.Messages.DivorceRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            if (update.Message?.ReplyToMessage is null)
            {
                await SendReplyMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Constants.Marriage.ReplyKeys.Divorce));

                return;
            }

            var menu = MarriageMenuGenerator.GenerateConfirmationMenu(update);

            var message = await SendMessage(
                botClient,
                update,
                Constants.Marriage.Messages.ComposeMarryOrDivorceProposal(
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
