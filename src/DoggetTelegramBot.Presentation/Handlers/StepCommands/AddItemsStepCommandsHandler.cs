using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;
using DoggetTelegramBot.Presentation.Handlers.Common.Caches;
using DoggetTelegramBot.Presentation.Handlers.Common.Enums;
using DoggetTelegramBot.Presentation.Helpers.MenuGenerators;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Presentation.Helpers.Common;

namespace DoggetTelegramBot.Presentation.Handlers.StepCommands
{
    public sealed class AddItemsStepCommandsHandler
    {
        [InlineCallbackHandler<AddItemsStepCommands>(AddItemsStepCommands.SelectItemType)]
        public static async Task SelectItemType(ITelegramBotClient botClient, Update update)
        {
            InlineCallback<EntityTCommand<ItemType>>? command = InlineCallback<EntityTCommand<ItemType>>
                 .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            var userChoice = command.Data.EntityId;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<AddItemsStepCache>();
            cache.Type = userChoice;

            handler.RegisterNextStep(SelectOrWriteServerName);

            var serverNameMessageOptions = AddItemsMenuGenerator
                .GenerateServerNamesReplyMenu(cache.ServerNames);

            await PRTelegramBot.Helpers.Message.DeleteMessage(
                botClient,
                update.GetChatId(),
                update.GetMessageId());

            await PRTelegramBot.Helpers.Message.Send(
                botClient,
                update,
                Constants.Item.Messages.Add.SelectItemServerNameRequest(userChoice),
                serverNameMessageOptions);
        }

        public static async Task SelectOrWriteServerName(ITelegramBotClient botClient, Update update)
        {
            string userChoice = update.Message!.Text!;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<AddItemsStepCache>();
            cache.ServerName = char.ToUpper(userChoice[0]) + userChoice[1..].ToLower();

            handler.RegisterNextStep(SelectItemAmountType);

            var selectItemAmountTypeMessageOptions = AddItemsMenuGenerator.GenerateItemAmountTypeMenu();

            await PRTelegramBot.Helpers.Message.Send(
                botClient,
                update,
                Constants.Item.Messages.SelectItemAmountTypeRequest(cache.ServerName),
                selectItemAmountTypeMessageOptions);
        }

        [InlineCallbackHandler<AddItemsStepCommands>(AddItemsStepCommands.SelectItemAmountType)]
        public static async Task SelectItemAmountType(ITelegramBotClient botClient, Update update)
        {
            InlineCallback<EntityTCommand<ItemAmountType>>? command = InlineCallback<EntityTCommand<ItemAmountType>>
                 .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            var userChoice = command.Data.EntityId;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<AddItemsStepCache>();
            cache.AmountType = userChoice;

            handler.RegisterNextStep(WriteValuesForAmountType);

            await PRTelegramBot.Helpers.Message.DeleteMessage(
                botClient,
                update.GetChatId(),
                update.GetMessageId());

            await PRTelegramBot.Helpers.Message.Send(
                botClient,
                update,
                Constants.Item.Messages.Add.WriteValuesForAmountTypeRequest(userChoice),
                new OptionMessage { ClearMenu = true });
        }

        public static async Task WriteValuesForAmountType(ITelegramBotClient botClient, Update update)
        {
            string? text = update.Message?.Text;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<AddItemsStepCache>();

            if (!cache.Values.TryGetValue(cache.AmountType, out _))
            {
                cache.Values[cache.AmountType] = [];
            }

            var newValues = await GetValues(text);
            cache.Values[cache.AmountType].AddRange(newValues);

            handler.RegisterNextStep(SelectMoreAmountTypes);

            var selectNextOrPreviousStepMessageOptions = AddItemsMenuGenerator
                .GenerateMoreAmountTypesConfirmationMenu();

            await PRTelegramBot.Helpers.Message.Send(
                botClient,
                update,
                Constants.Item.Messages.Add.SelectMoreAmountTypesRequest,
                selectNextOrPreviousStepMessageOptions);
        }

        [InlineCallbackHandler<SelectAmountTypeConfirmationCommands>(SelectAmountTypeConfirmationCommands.Yes, SelectAmountTypeConfirmationCommands.No)]
        public static async Task SelectMoreAmountTypes(ITelegramBotClient botClient, Update update)
        {
            InlineCallback<EntityTCommand<bool>>? command = InlineCallback<EntityTCommand<bool>>
                 .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            bool userChoice = command.Data.EntityId;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<AddItemsStepCache>();

            if (!userChoice)
            {
                await HandleSuccessfulAddConfirmationAsync(botClient, update, cache, handler);
                return;
            }

            handler.RegisterNextStep(SelectItemAmountType);

            var selectItemAmountTypeMessageOptions = AddItemsMenuGenerator.GenerateItemAmountTypeMenu();

            await PRTelegramBot.Helpers.Message.DeleteMessage(
                botClient,
                update.GetChatId(),
                update.GetMessageId());

            await PRTelegramBot.Helpers.Message.Send(
                botClient,
                update,
                Constants.Item.Messages.Add.SelectMoreAmountTypes,
                selectItemAmountTypeMessageOptions);
        }

        private static async Task HandleSuccessfulAddConfirmationAsync(
            ITelegramBotClient botClient,
            Update update,
            AddItemsStepCache cache,
            StepTelegram handler)
        {
            await PRTelegramBot.Helpers.Message.Edit(
                botClient,
                update,
                Constants.Item.Messages.Add.SuccessfulConfirmation(
                    cache.Type, cache.ServerName, cache.Values));

            ConfirmationState<AddItemsStepCache>.SetUserResponse(update.CallbackQuery!.From!.Id, cache);

            StepFinalizer.FinalizeStep(update, handler);
        }

        private static async Task<List<string>> GetValues(string? text) =>
            await Task.FromResult(string.IsNullOrEmpty(text) ?
                [] :
                text.Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList());
    }
}
