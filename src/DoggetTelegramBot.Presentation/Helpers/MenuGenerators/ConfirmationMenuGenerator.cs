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
    public sealed class ConfirmationMenuGenerator
    {
        public static OptionMessage Generate(Update update)
        {
            InlineCallback<EntityTCommand<bool>> yes = new(
                UserConfirmationCommand.Yes.GetDescription(),
                UserConfirmationCommand.Yes,
                new EntityTCommand<bool>(true));

            InlineCallback<EntityTCommand<bool>> no = new(
                UserConfirmationCommand.No.GetDescription(),
                UserConfirmationCommand.No,
                new EntityTCommand<bool>(false));

            List<IInlineContent> menu = [yes, no];

            var testMenu = MenuGenerator.InlineKeyboard(2, menu);

            OptionMessage options = new()
            {
                MenuInlineKeyboardMarkup = testMenu,
                ReplyToMessageId = update.Message?.ReplyToMessage?.MessageId
            };

            return options;
        }
    }
}
