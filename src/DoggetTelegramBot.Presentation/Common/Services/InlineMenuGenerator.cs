using PRTelegramBot.Interfaces;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using PRTelegramBot.Extensions;
using Telegram.Bot.Types;
using DoggetTelegramBot.Domain.Common.Enums;

namespace DoggetTelegramBot.Presentation.Common.Services
{
    public class InlineMenuGenerator
    {
        public static OptionMessage GenerateYesNoMenu(Update update)
        {
            InlineCallback<EntityTCommand<bool>> yes = new(
                UserResponse.Yes.GetDescription(), UserResponse.Yes, new EntityTCommand<bool>(true));

            InlineCallback<EntityTCommand<bool>> no = new(
                UserResponse.No.GetDescription(), UserResponse.No, new EntityTCommand<bool>(false));

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
