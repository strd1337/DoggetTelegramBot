using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Commands.Create;
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

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.Marriage.ReplyKeys.CreateMarriage)]
        public async Task Create(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Marriage.Messages.CreateMarriageRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            if (update.Message?.ReplyToMessage is null)
            {
                await SendMessage(botClient, update, Constants.Marriage.Messages.NotFoundUserReply);
                return;
            }

            var menu = InlineMenuGenerator.GenerateYesNoMenu(update);

            var message = await PRTelegramBot.Helpers.Message.Send(
                botClient,
                update,
                Constants.Marriage.Messages.ComposeMarriageProposal(
                    update.Message.From!.FirstName,
                    update.Message.From!.Username,
                    update.Message.ReplyToMessage.From!.FirstName,
                    update.Message.ReplyToMessage.From.Username),
                menu);

            _ = HandleUserMarriageRequestAsync(botClient, update, message);
        }

        private async Task HandleUserMarriageRequestAsync(
            ITelegramBotClient botClient,
            Update update,
            Message message)
        {
            bool? userResponse = await UserState.WaitForResponseAsync(update.Message!.From!.Id);

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

                CreateMarriageCommand command = new(
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

                logger.LogCommon(
                    Constants.Marriage.Messages.CreateMarriageRequest(false),
                    TelegramEvents.Message,
                    Constants.LogColors.Request);
            }
            else
            {
                await EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    Constants.Marriage.Messages.DenyMarriageRequest(
                        message.ReplyToMessage.From!.FirstName,
                        message.ReplyToMessage.From!.Username));
            }
        }
    }
}
