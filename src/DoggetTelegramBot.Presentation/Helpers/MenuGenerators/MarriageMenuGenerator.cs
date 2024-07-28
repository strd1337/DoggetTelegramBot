using PRTelegramBot.Interfaces;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using PRTelegramBot.Extensions;
using Telegram.Bot.Types;
using DoggetTelegramBot.Presentation.Handlers.Common.Enums;

namespace DoggetTelegramBot.Presentation.Helpers.MenuGenerators
{
    public sealed class MarriageMenuGenerator
    {
        public static OptionMessage GenerateConfirmationMenu(Update update, int maxColumn = 2)
        {
            InlineCallback<EntityTCommand<bool>> yes = new(
                MarriageConfirmationCommands.Yes.GetDescription(),
                MarriageConfirmationCommands.Yes,
                new EntityTCommand<bool>(true));

            InlineCallback<EntityTCommand<bool>> no = new(
                MarriageConfirmationCommands.No.GetDescription(),
                MarriageConfirmationCommands.No,
                new EntityTCommand<bool>(false));

            List<IInlineContent> menu = [yes, no];

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
