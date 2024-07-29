using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Commands.Add;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.Common.Mapping;
using DoggetTelegramBot.Presentation.Common.Services;
using DoggetTelegramBot.Presentation.Handlers.Common.Caches;
using DoggetTelegramBot.Presentation.Helpers.Common;
using MapsterMapper;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.Handlers.Requests
{
    public sealed class FamilyRequestHandler(
        IScopeService scopeService,
        IMapper mapper,
        IBotLogger logger,
        ITelegramBotService botService)
    {
        public async Task HandleAddToFamilyAsync(ITelegramBotClient botClient, Update update, Message message)
        {
            long parentTelegramId = update.Message!.From!.Id;

            var userResponse = await ConfirmationState<AddToFamilyStepCache>.WaitForResponseAsync(
                parentTelegramId, Constants.Family.AddToFamilyTimeoutInSeconds);

            if (userResponse is null)
            {
                await botService.EditMessage(
                    botClient,
                    message.Chat.Id,
                    message.MessageId,
                    Constants.Messages.TimeExpired);
            }
            else
            {
                long newMemberTelegramId = update.Message.ReplyToMessage!.From!.Id;

                AddToFamilyCommand command = new(
                    parentTelegramId,
                    newMemberTelegramId,
                    userResponse.FamilyRole);

                var result = await scopeService.Send(command);

                var response = result.Match(mapper.Map<Response>, botService.Problem);

                await botService.SendMessage(
                    botClient,
                    update,
                    response.Message);
            }

            logger.LogCommon(
                Constants.Family.Messages.AddToFamilyRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);
        }
    }
}
