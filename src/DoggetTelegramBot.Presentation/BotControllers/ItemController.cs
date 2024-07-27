using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Items.Queries.Server;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Services;
using DoggetTelegramBot.Presentation.Handlers.Common.Caches;
using DoggetTelegramBot.Presentation.Handlers.Requests;
using DoggetTelegramBot.Presentation.Handlers.StepCommands;
using DoggetTelegramBot.Presentation.Helpers.MenuGenerators;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public class ItemController(
        IBotLogger logger,
        IScopeService service,
        ITelegramBotService botService,
        ItemRequestHandler requestHandler) : BaseController(botService)
    {
        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.Item.ReplyKeys.Purchase)]
        [RequiredTypeChat(ChatType.Private)]
        public async Task Purchase(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Item.Messages.PurchaseRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            var serverNames = await GetServerNamesAsync();

            if (serverNames.Count == 0)
            {
                await SendMessage(
                    botClient,
                    update,
                    Constants.Item.Messages.Purchase.ServerNamesNotFound);

                return;
            }

            BuyItemStepCache cache = new();
            cache.AddServerNames(serverNames);

            StepTelegram handler = new(
                BuyItemStepCommandsHandler.SelectItemType,
                cache);

            update.RegisterStepHandler(handler);

            var selectItemTypeMessageOptions = BuyItemMenuGenerator.GenerateItemTypeMenu();

            var message = await SendMessage(
                botClient,
                update,
                Constants.Item.Messages.SelectItemTypeRequest(),
                selectItemTypeMessageOptions);

            _ = requestHandler.HandlePurchaseAsync(botClient, update, message);
        }

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.Item.ReplyKeys.Add)]
        [RequiredTypeChat(ChatType.Private)]
        [Access((int)UserPrivilege.Admin)]
        public async Task Add(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Item.Messages.AddRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            var serverNames = await GetServerNamesAsync();

            AddItemsStepCache cache = new();
            cache.AddServerNames(serverNames);

            StepTelegram handler = new(
                AddItemsStepCommandsHandler.SelectItemType,
                cache);

            update.RegisterStepHandler(handler);

            var selectItemTypeMessageOptions = AddItemsMenuGenerator.GenerateItemTypeMenu();

            var message = await SendMessage(
                botClient,
                update,
                Constants.Item.Messages.SelectItemTypeRequest(false),
                selectItemTypeMessageOptions);

            _ = requestHandler.HandleAddAsync(botClient, update, message);
        }

        private async Task<List<string>> GetServerNamesAsync()
        {
            logger.LogCommon(
                Constants.Item.Messages.GetAllItemServerNamesRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            GetAllItemServerNamesQuery query = new();
            var serverNamesResult = await service.Send(query);

            logger.LogCommon(
                Constants.Item.Messages.GetAllItemServerNamesRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return serverNamesResult.Value.ServerNames;
        }
    }
}
