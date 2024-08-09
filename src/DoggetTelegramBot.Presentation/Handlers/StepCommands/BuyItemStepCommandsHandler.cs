using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;
using DoggetTelegramBot.Presentation.Handlers.Common.Caches;
using DoggetTelegramBot.Presentation.Handlers.Common.Enums;
using DoggetTelegramBot.Presentation.Helpers.Common;
using DoggetTelegramBot.Presentation.Helpers.MenuGenerators;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot;
using Telegram.Bot.Types;
using ItemConstants = DoggetTelegramBot.Domain.Common.Constants.Item.Constants.Item;

namespace DoggetTelegramBot.Presentation.Handlers.StepCommands
{
    public sealed class BuyItemStepCommandsHandler
    {
        [InlineCallbackHandler<BuyItemStepCommands>(BuyItemStepCommands.SelectItemType)]
        public static async Task SelectItemType(ITelegramBotClient botClient, Update update)
        {
            InlineCallback<EntityTCommand<ItemType>>? command = InlineCallback<EntityTCommand<ItemType>>
                 .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            var userChoice = command.Data.EntityId;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<BuyItemStepCache>();
            cache.Type = userChoice;

            handler.RegisterNextStep(SelectItemServerName);

            var selectServerNameMessageOptions = BuyItemMenuGenerator
                .GenerateItemServerNamesInlineMenu(cache.ServerNames);

            await PRTelegramBot.Helpers.Message.Edit(
                botClient,
                update,
                ItemConstants.Purchase.Messages.SelectItemServerNameRequest(userChoice),
                selectServerNameMessageOptions);
        }

        [InlineCallbackHandler<BuyItemStepCommands>(BuyItemStepCommands.SelectItemServerName)]
        public static async Task SelectItemServerName(ITelegramBotClient botClient, Update update)
        {
            InlineCallback<EntityTCommand<string>>? command = InlineCallback<EntityTCommand<string>>
                 .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            string userChoice = command.Data.EntityId;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<BuyItemStepCache>();
            cache.ServerName = userChoice;

            if (cache.Type is not ItemType.PromoCode)
            {
                await HandleSuccessfulPurchaseConfirmationAsync(botClient, update, cache, handler);
                return;
            }

            handler.RegisterNextStep(SelectItemAmountType);

            var selectItemAmountTypeMessageOptions = BuyItemMenuGenerator.GenerateItemAmountTypeMenu();

            await PRTelegramBot.Helpers.Message.Edit(
                botClient,
                update,
                ItemConstants.Messages.SelectItemAmountTypeRequest(userChoice),
                selectItemAmountTypeMessageOptions);
        }

        [InlineCallbackHandler<BuyItemStepCommands>(BuyItemStepCommands.SelectItemAmountType)]
        public static async Task SelectItemAmountType(ITelegramBotClient botClient, Update update)
        {
            InlineCallback<EntityTCommand<ItemAmountType>>? command = InlineCallback<EntityTCommand<ItemAmountType>>
                 .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            var userChoice = command.Data.EntityId;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<BuyItemStepCache>();
            cache.AmountType = userChoice;

            await HandleSuccessfulPurchaseConfirmationAsync(botClient, update, cache, handler);
        }

        private static async Task HandleSuccessfulPurchaseConfirmationAsync(
            ITelegramBotClient botClient,
            Update update,
            BuyItemStepCache cache,
            StepTelegram handler)
        {
            await PRTelegramBot.Helpers.Message.Edit(
                botClient,
                update,
                ItemConstants.Purchase.Messages.SuccessfulConfirmation(
                    cache.Type, cache.ServerName, cache.AmountType));

            ConfirmationState<BuyItemStepCache>.SetUserResponse(update.CallbackQuery!.From!.Id, cache);

            StepFinalizer.FinalizeStep(update, handler);
        }
    }
}
