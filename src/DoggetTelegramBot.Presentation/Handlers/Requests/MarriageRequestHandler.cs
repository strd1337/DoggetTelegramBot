using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Commands.Create;
using DoggetTelegramBot.Application.Marriages.Commands.Delete;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using DoggetTelegramBot.Presentation.Common.Mapping;
using DoggetTelegramBot.Presentation.Common.Services;
using DoggetTelegramBot.Presentation.Helpers.Common;
using MapsterMapper;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.Handlers.Requests
{
    public sealed class MarriageRequestHandler(
        IScopeService scopeService,
        IMapper mapper,
        IBotLogger logger,
        ITelegramBotService botService)
    {
        public async Task HandleGetMarriedAsync(
            ITelegramBotClient botClient,
            Update update,
            Message message)
        {
            bool? userResponse = await ConfirmationState<bool>.WaitForResponseAsync(
                update.Message!.ReplyToMessage!.From!.Id);

            if (userResponse.HasValue && !userResponse.Value)
            {
                await botService.EditMessage(
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

                var result = await scopeService.Send(command);

                var response = result.Match(mapper.Map<Response>, botService.Problem);

                await botService.EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    response.Message);
            }
            else
            {
                await botService.EditMessage(
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

        public async Task HandleDivorceAsync(
            ITelegramBotClient botClient,
            Update update,
            Message message)
        {
            bool? userResponse = await ConfirmationState<bool>.WaitForResponseAsync(
               update.Message!.ReplyToMessage!.From!.Id);

            if (userResponse.HasValue && !userResponse.Value)
            {
                await botService.EditMessage(
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

                var result = await scopeService.Send(command);

                var response = result.Match(mapper.Map<Response>, botService.Problem);

                await botService.EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    response.Message);
            }
            else
            {
                await botService.EditMessage(
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
