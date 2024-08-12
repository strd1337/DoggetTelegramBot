using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Items.Queries.Server;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using DoggetTelegramBot.Presentation.BotCommands;
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
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using ItemConstants = DoggetTelegramBot.Domain.Common.Constants.Item.Constants.Item;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public class ItemController(
        IBotLogger logger,
        IScopeService service,
        ITelegramBotService botService,
        ItemRequestHandler requestHandler) : BaseController(botService)
    {
        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.Item.Purchase)]
        [RequiredTypeChat(ChatType.Private)]
        public async Task PurchaseAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                ItemConstants.Requests.Purchase(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            var serverNames = await GetServerNamesAsync();

            if (serverNames.Count == 0)
            {
                await SendMessage(
                    botClient,
                    update,
                    ItemConstants.Purchase.Messages.ServerNamesNotFound);

                logger.LogCommon(
                    ItemConstants.Requests.Purchase(false),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.Request);

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
                ItemConstants.Messages.SelectItemTypeRequest(),
                selectItemTypeMessageOptions);

            _ = requestHandler.HandlePurchaseAsync(botClient, update, message);
        }

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.Item.Add)]
        [RequiredTypeChat(ChatType.Private)]
        [Access((int)UserPrivilege.Admin)]
        public async Task AddAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                ItemConstants.Requests.Add(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

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
                ItemConstants.Messages.SelectItemTypeRequest(false),
                selectItemTypeMessageOptions);

            _ = requestHandler.HandleAddAsync(botClient, update, message);
        }

        private async Task<List<string>> GetServerNamesAsync()
        {
            logger.LogCommon(
                ItemConstants.Requests.GetAllItemServerNames(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetAllItemServerNamesQuery query = new();
            var serverNamesResult = await service.Send(query);

            logger.LogCommon(
                ItemConstants.Requests.GetAllItemServerNames(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return serverNamesResult.Value.ServerNames;
        }
    }
}
