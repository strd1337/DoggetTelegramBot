using PRTelegramBot.Interfaces;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;
using PRTelegramBot.Extensions;
using DoggetTelegramBot.Presentation.Handlers.Common.Enums;

namespace DoggetTelegramBot.Presentation.Helpers.MenuGenerators
{
    public class BuyItemMenuGenerator
    {
        public static OptionMessage GenerateItemTypeMenu(int maxColumn = 3)
        {
            InlineCallback<EntityTCommand<ItemType>> promoCode = new(
                ItemType.PromoCode.GetDescription(),
                BuyItemStepCommands.SelectItemType,
                new EntityTCommand<ItemType>(ItemType.PromoCode));

            List<IInlineContent> menu = [promoCode];

            var inlineKeyboardMenu = MenuGenerator.InlineKeyboard(maxColumn, menu);

            OptionMessage options = new()
            {
                MenuInlineKeyboardMarkup = inlineKeyboardMenu,
            };

            return options;
        }

        public static OptionMessage GenerateItemServerNamesMenu(
            IReadOnlyList<string> serverNames,
            int maxColumn = 3)
        {
            List<IInlineContent> menu = [];
            foreach (string name in serverNames)
            {
                InlineCallback<EntityTCommand<string>> serverName = new(
                    name,
                    BuyItemStepCommands.SelectItemServerName,
                    new EntityTCommand<string>(name));

                menu.Add(serverName);
            };

            var inlineKeyboardMenu = MenuGenerator.InlineKeyboard(maxColumn, menu);

            return new OptionMessage
            {
                MenuInlineKeyboardMarkup = inlineKeyboardMenu
            };
        }

        public static OptionMessage GenerateItemAmountTypeMenu(int maxColumn = 3)
        {
            InlineCallback<EntityTCommand<ItemAmountType>> ten = new(
                ItemAmountType.Ten.GetDescription(),
                BuyItemStepCommands.SelectItemAmountType,
                new EntityTCommand<ItemAmountType>(ItemAmountType.Ten));

            InlineCallback<EntityTCommand<ItemAmountType>> twelve = new(
                ItemAmountType.Twelve.GetDescription(),
                BuyItemStepCommands.SelectItemAmountType,
                new EntityTCommand<ItemAmountType>(ItemAmountType.Twelve));

            InlineCallback<EntityTCommand<ItemAmountType>> fifty = new(
                ItemAmountType.Fifty.GetDescription(),
                BuyItemStepCommands.SelectItemAmountType,
                new EntityTCommand<ItemAmountType>(ItemAmountType.Fifty));

            InlineCallback<EntityTCommand<ItemAmountType>> oneHundred = new(
                ItemAmountType.OneHundred.GetDescription(),
                BuyItemStepCommands.SelectItemAmountType,
                new EntityTCommand<ItemAmountType>(ItemAmountType.OneHundred));

            List<IInlineContent> menu = [ten, twelve, fifty, oneHundred];

            var inlineKeyboardMenu = MenuGenerator.InlineKeyboard(maxColumn, menu);

            OptionMessage options = new()
            {
                MenuInlineKeyboardMarkup = inlineKeyboardMenu,
            };

            return options;
        }
    }
}
