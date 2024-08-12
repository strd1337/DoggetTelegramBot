using DoggetTelegramBot.Presentation.Handlers.Common.Enums;
using PRTelegramBot.Interfaces;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot.Types;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using PRTelegramBot.Extensions;

namespace DoggetTelegramBot.Presentation.Helpers.MenuGenerators
{
    public static class MarryMenuGenerator
    {
        public static OptionMessage GenerateConfirmationMenu(Update update, int maxColumn = 2)
        {
            InlineCallback<EntityTCommand<MarriageConfirmationCommands>> yes = new(
                MarriageConfirmationCommands.Yes.GetDescription(),
                MarryCommands.SelectConfirmationType,
                new EntityTCommand<MarriageConfirmationCommands>(MarriageConfirmationCommands.Yes));

            InlineCallback<EntityTCommand<MarriageConfirmationCommands>> no = new(
                MarriageConfirmationCommands.No.GetDescription(),
                MarryCommands.SelectConfirmationType,
                new EntityTCommand<MarriageConfirmationCommands>(MarriageConfirmationCommands.No));

            List<IInlineContent> menu = [yes, no];

            var testMenu = MenuGenerator.InlineKeyboard(maxColumn, menu);

            OptionMessage options = new()
            {
                MenuInlineKeyboardMarkup = testMenu,
                ReplyToMessageId = update.Message?.ReplyToMessage?.MessageId
            };

            return options;
        }

        public static OptionMessage GenerateMarriageTypeMenu(Update update, int maxColumn = 4)
        {
            InlineCallback<EntityTCommand<MarriageType>> civil = new(
                MarriageType.Civil.GetDescription(),
                MarryCommands.SelectMarriageType,
                new EntityTCommand<MarriageType>(MarriageType.Civil));

            InlineCallback<EntityTCommand<MarriageType>> religious = new(
               MarriageType.Religious.GetDescription(),
               MarryCommands.SelectMarriageType,
               new EntityTCommand<MarriageType>(MarriageType.Religious));

            InlineCallback<EntityTCommand<MarriageType>> sameSex = new(
               MarriageType.SameSex.GetDescription(),
               MarryCommands.SelectMarriageType,
               new EntityTCommand<MarriageType>(MarriageType.SameSex));

            InlineCallback<EntityTCommand<MarriageType>> commonLaw = new(
               MarriageType.CommonLaw.GetDescription(),
               MarryCommands.SelectMarriageType,
               new EntityTCommand<MarriageType>(MarriageType.CommonLaw));

            List<IInlineContent> menu = [civil, commonLaw, religious, sameSex];

            var testMenu = MenuGenerator.InlineKeyboard(maxColumn, menu);

            OptionMessage options = new()
            {
                MenuInlineKeyboardMarkup = testMenu,
                ReplyToMessageId = update.Message?.ReplyToMessage?.MessageId
            };

            return options;
        }
    }
}
