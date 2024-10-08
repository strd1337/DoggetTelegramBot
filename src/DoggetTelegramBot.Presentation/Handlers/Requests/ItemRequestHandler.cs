using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Items.Commands.Add;
using DoggetTelegramBot.Application.Items.Commands.Purchase;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.Common.Mapping;
using DoggetTelegramBot.Presentation.Common.Services;
using DoggetTelegramBot.Presentation.Handlers.Common.Caches;
using DoggetTelegramBot.Presentation.Helpers.Common;
using MapsterMapper;
using Telegram.Bot;
using Telegram.Bot.Types;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using ItemConstants = DoggetTelegramBot.Domain.Common.Constants.Item.Constants.Item;

namespace DoggetTelegramBot.Presentation.Handlers.Requests
{
    public sealed class ItemRequestHandler(
        IScopeService scopeService,
        IMapper mapper,
        IBotLogger logger,
        ITelegramBotService botService)
    {
        public async Task HandlePurchaseAsync(ITelegramBotClient botClient, Update update, Message message)
        {
            long userTelegramId = update.Message!.From!.Id;

            var userResponse = await ConfirmationState<BuyItemStepCache>.WaitForResponseAsync(
                userTelegramId, ItemConstants.PurchaseTimeoutInSeconds);

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
                PurchaseItemCommand command = new(
                    userTelegramId,
                    userResponse.Type,
                    userResponse.ServerName,
                    userResponse.AmountType,
                    userResponse.Count);

                var result = await scopeService.Send(command);

                var response = result.Match(mapper.Map<Response>, botService.Problem);

                await botService.SendMessage(
                    botClient,
                    update,
                    response.Message);
            }

            logger.LogCommon(
                ItemConstants.Requests.Purchase(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);
        }

        public async Task HandleAddAsync(ITelegramBotClient botClient, Update update, Message message)
        {
            long userTelegramId = update.Message!.From!.Id;

            var userResponse = await ConfirmationState<AddItemsStepCache>.WaitForResponseAsync(
                userTelegramId, ItemConstants.AddTimeoutInSeconds);

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
                AddItemsCommand command = new(
                    userResponse.Type,
                    userResponse.ServerName,
                    userResponse.Values);

                var result = await scopeService.Send(command);

                var response = result.Match(mapper.Map<Response>, botService.Problem);

                await botService.SendMessage(
                    botClient,
                    update,
                    response.Message);
            }

            logger.LogCommon(
                ItemConstants.Requests.Add(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);
        }
    }
}
