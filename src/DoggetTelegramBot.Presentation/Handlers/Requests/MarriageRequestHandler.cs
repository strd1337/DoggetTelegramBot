using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Commands.Marry;
using DoggetTelegramBot.Application.Marriages.Commands.Divorce;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.Common.Mapping;
using DoggetTelegramBot.Presentation.Common.Services;
using DoggetTelegramBot.Presentation.Helpers.Common;
using MapsterMapper;
using Telegram.Bot;
using Telegram.Bot.Types;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using MarriageConstants = DoggetTelegramBot.Domain.Common.Constants.Marriage.Constants.Marriage;
using DoggetTelegramBot.Presentation.Handlers.Common.Caches;
using DoggetTelegramBot.Presentation.Handlers.Common.Enums;

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
            var userResponse = await ConfirmationState<MarryStepCache>.WaitForResponseAsync(
                update.Message!.ReplyToMessage!.From!.Id,
                MarriageConstants.MarryTimeoutInSeconds);

            if (userResponse is null)
            {
                await botService.EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    Constants.Messages.TimeExpired);
            }
            else if (userResponse.ConfirmationCommand is MarriageConfirmationCommands.Yes)
            {
                List<long> spouses =
                [
                    update.Message!.From!.Id,
                    update.Message!.ReplyToMessage!.From!.Id
                ];

                MarryCommand command = new(
                    userResponse.MarriageType,
                    spouses);

                var result = await scopeService.Send(command);

                var response = result.Match(mapper.Map<Response>, botService.Problem);

                await botService.SendMessage(
                    botClient,
                    update,
                    response.Message);
            }
            else
            {
                await botService.EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    message.ReplyToMessage!.MessageId,
                    MarriageConstants.Messages.DenyMarryOrDivorceRequest(
                        message.ReplyToMessage.From!.FirstName,
                        message.ReplyToMessage.From!.Username));
            }

            logger.LogCommon(
                MarriageConstants.Requests.Marry(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);
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
                    MarriageConstants.Messages.DenyMarryOrDivorceRequest(
                        message.ReplyToMessage.From!.FirstName,
                        message.ReplyToMessage.From!.Username,
                        false));
            }

            logger.LogCommon(
                MarriageConstants.Requests.Divorce(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);
        }
    }
}
