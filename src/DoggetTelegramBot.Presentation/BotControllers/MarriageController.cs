using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Commands.Create;
using DoggetTelegramBot.Application.Marriages.Commands.Delete;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Mapping;
using DoggetTelegramBot.Presentation.Common.Services;
using MapsterMapper;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public class MarriageController(
        IBotLogger logger,
        IScopeService service,
        IMapper mapper) : BaseController(logger)
    {
        private readonly IBotLogger logger = logger;

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
                await SendMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Constants.Marriage.ReplyKeys.Marry));

                return;
            }

            var menu = InlineMenuGenerator.GenerateYesNoMenu(update);

            var message = await PRTelegramBot.Helpers.Message.Send(
                botClient,
                update,
                Constants.Marriage.Messages.ComposeMarryOrDivorceProposal(
                    update.Message.From!.FirstName,
                    update.Message.From!.Username,
                    update.Message.ReplyToMessage.From!.FirstName,
                    update.Message.ReplyToMessage.From.Username),
                menu);

            _ = HandleMarryRequestAsync(botClient, update, message);
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
                await SendMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Constants.Marriage.ReplyKeys.Divorce));

                return;
            }

            var menu = InlineMenuGenerator.GenerateYesNoMenu(update);

            var message = await PRTelegramBot.Helpers.Message.Send(
                botClient,
                update,
                Constants.Marriage.Messages.ComposeMarryOrDivorceProposal(
                    update.Message.From!.FirstName,
                    update.Message.From!.Username,
                    update.Message.ReplyToMessage.From!.FirstName,
                    update.Message.ReplyToMessage.From.Username,
                    false),
                menu);

            _ = HandleDivorceRequestAsync(botClient, update, message);
        }

        private async Task HandleMarryRequestAsync(
            ITelegramBotClient botClient,
            Update update,
            Message message)
        {
            bool? userResponse = await UserState.WaitForResponseAsync(
                update.Message!.ReplyToMessage!.From!.Id);

            if (!userResponse.HasValue)
            {
                await EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    Constants.Messages.TimeExpired);
            }
            else if (userResponse.Value)
            {
                List<long> spouses =
                [
                    update.Message!.From!.Id,
                    update.Message!.ReplyToMessage!.From!.Id
                ];

                MarryCommand command = new(
                    MarriageType.Civil,
                    spouses);

                var result = await service.Send(command);

                var response = result.Match(mapper.Map<Response>, Problem);

                await EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    response.Message);
            }
            else
            {
                await EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    Constants.Marriage.Messages.DenyMarryOrDivorceRequest(
                        message.ReplyToMessage.From!.FirstName,
                        message.ReplyToMessage.From!.Username));
            }

            logger.LogCommon(
                Constants.Marriage.Messages.MarryRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);
        }

        private async Task HandleDivorceRequestAsync(
            ITelegramBotClient botClient,
            Update update,
            Message message)
        {
            bool? userResponse = await UserState.WaitForResponseAsync(
                update.Message!.ReplyToMessage!.From!.Id);

            if (!userResponse.HasValue)
            {
                await EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    Constants.Messages.TimeExpired);
            }
            else if (userResponse.Value)
            {
                List<long> spouses =
                [
                    update.Message!.From!.Id,
                    update.Message!.ReplyToMessage!.From!.Id
                ];

                DivorceCommand command = new(spouses);

                var result = await service.Send(command);

                var response = result.Match(mapper.Map<Response>, Problem);

                await EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    response.Message);
            }
            else
            {
                await EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    Constants.Marriage.Messages.DenyMarryOrDivorceRequest(
                        message.ReplyToMessage.From!.FirstName,
                        message.ReplyToMessage.From!.Username,
                        false));
            }

            logger.LogCommon(
                Constants.Marriage.Messages.DivorceRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);
        }
    }
}
