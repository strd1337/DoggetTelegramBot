using DoggetTelegramBot.Presentation.Handlers.Common.Enums;
using PRTelegramBot.Interfaces;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using PRTelegramBot.Extensions;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.Helpers.MenuGenerators
{
    public static class AddToFamilyMenuGenerator
    {
        public static OptionMessage GenerateFamilyRoleSelectionMenu(Update update, int maxColumn = 4)
        {
            InlineCallback<EntityTCommand<FamilyRole>> daughter = new(
                FamilyRole.Daughter.GetDescription(),
                AddToFamilyStepCommands.SelectFamilyRole,
                new EntityTCommand<FamilyRole>(FamilyRole.Daughter));

            InlineCallback<EntityTCommand<FamilyRole>> son = new(
                FamilyRole.Son.GetDescription(),
                AddToFamilyStepCommands.SelectFamilyRole,
                new EntityTCommand<FamilyRole>(FamilyRole.Son));

            InlineCallback<EntityTCommand<FamilyRole>> dog = new(
                FamilyRole.Dog.GetDescription(),
                AddToFamilyStepCommands.SelectFamilyRole,
                new EntityTCommand<FamilyRole>(FamilyRole.Dog));

            InlineCallback<EntityTCommand<FamilyRole>> cat = new(
                FamilyRole.Cat.GetDescription(),
                AddToFamilyStepCommands.SelectFamilyRole,
                new EntityTCommand<FamilyRole>(FamilyRole.Cat));

            List<IInlineContent> menu = [daughter, son, dog, cat];

            var inlineKeyboardMenu = MenuGenerator.InlineKeyboard(maxColumn, menu);

            OptionMessage options = new()
            {
                MenuInlineKeyboardMarkup = inlineKeyboardMenu,
                ReplyToMessageId = update.Message?.MessageId
            };

            return options;
        }
    }
}
