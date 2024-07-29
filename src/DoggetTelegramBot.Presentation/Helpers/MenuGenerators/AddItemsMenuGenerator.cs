using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;
using DoggetTelegramBot.Presentation.Handlers.Common.Enums;
using PRTelegramBot.Interfaces;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot.Types.ReplyMarkups;
using PRTelegramBot.Extensions;

namespace DoggetTelegramBot.Presentation.Helpers.MenuGenerators
{
    public static class AddItemsMenuGenerator
    {
        public static OptionMessage GenerateItemTypeMenu(int maxColumn = 3)
        {
            InlineCallback<EntityTCommand<ItemType>> promoCode = new(
                ItemType.PromoCode.GetDescription(),
                AddItemsStepCommands.SelectItemType,
                new EntityTCommand<ItemType>(ItemType.PromoCode));

            List<IInlineContent> menu = [promoCode];

            var inlineKeyboardMenu = MenuGenerator.InlineKeyboard(maxColumn, menu);

            OptionMessage options = new()
            {
                MenuInlineKeyboardMarkup = inlineKeyboardMenu,
            };

            return options;
        }

        public static OptionMessage GenerateServerNamesReplyMenu(
             IReadOnlyList<string> serverNames,
             int maxColumn = 3)
        {
            List<KeyboardButton> menu = [];
            foreach (string name in serverNames)
            {
                KeyboardButton serverName = new(name);

                menu.Add(serverName);
            };

            var replyKeyboardMenu = MenuGenerator.ReplyKeyboard(maxColumn, menu);

            return new OptionMessage
            {
                MenuReplyKeyboardMarkup = replyKeyboardMenu
            };
        }

        public static OptionMessage GenerateItemAmountTypeMenu(int maxColumn = 3)
        {
            InlineCallback<EntityTCommand<ItemAmountType>> ten = new(
                ItemAmountType.Ten.GetDescription(),
                AddItemsStepCommands.SelectItemAmountType,
                new EntityTCommand<ItemAmountType>(ItemAmountType.Ten));

            InlineCallback<EntityTCommand<ItemAmountType>> twelve = new(
                ItemAmountType.Twelve.GetDescription(),
                AddItemsStepCommands.SelectItemAmountType,
                new EntityTCommand<ItemAmountType>(ItemAmountType.Twelve));

            InlineCallback<EntityTCommand<ItemAmountType>> fifty = new(
                ItemAmountType.Fifty.GetDescription(),
                AddItemsStepCommands.SelectItemAmountType,
                new EntityTCommand<ItemAmountType>(ItemAmountType.Fifty));

            InlineCallback<EntityTCommand<ItemAmountType>> oneHundred = new(
                ItemAmountType.OneHundred.GetDescription(),
                AddItemsStepCommands.SelectItemAmountType,
                new EntityTCommand<ItemAmountType>(ItemAmountType.OneHundred));

            InlineCallback<EntityTCommand<ItemAmountType>> twoHundred = new(
                ItemAmountType.TwoHundred.GetDescription(),
                AddItemsStepCommands.SelectItemAmountType,
                new EntityTCommand<ItemAmountType>(ItemAmountType.TwoHundred));

            List<IInlineContent> menu = [ten, twelve, fifty, oneHundred, twoHundred];

            var inlineKeyboardMenu = MenuGenerator.InlineKeyboard(maxColumn, menu);

            OptionMessage options = new()
            {
                MenuInlineKeyboardMarkup = inlineKeyboardMenu,
            };

            return options;
        }

        public static OptionMessage GenerateMoreAmountTypesConfirmationMenu(int maxColumn = 2)
        {
            InlineCallback<EntityTCommand<bool>> yes = new(
                SelectAmountTypeConfirmationCommands.Yes.GetDescription(),
                SelectAmountTypeConfirmationCommands.Yes,
                new EntityTCommand<bool>(true));

            InlineCallback<EntityTCommand<bool>> no = new(
                SelectAmountTypeConfirmationCommands.No.GetDescription(),
                SelectAmountTypeConfirmationCommands.No,
                new EntityTCommand<bool>(false));

            List<IInlineContent> menu = [yes, no];

            var inlineKeyboard = MenuGenerator.InlineKeyboard(maxColumn, menu);

            OptionMessage options = new()
            {
                MenuInlineKeyboardMarkup = inlineKeyboard
            };

            return options;
        }
    }
}
