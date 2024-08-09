using DoggetTelegramBot.Presentation.Handlers.Common.Caches;
using DoggetTelegramBot.Presentation.Handlers.Common.Enums;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using DoggetTelegramBot.Presentation.Helpers.Common;
using FamilyConstants = DoggetTelegramBot.Domain.Common.Constants.Family.Constants.Family;

namespace DoggetTelegramBot.Presentation.Handlers.StepCommands
{
    public sealed class AddToFamilyStepCommandsHandler
    {
        [InlineCallbackHandler<AddToFamilyStepCommands>(AddToFamilyStepCommands.SelectFamilyRole)]
        public static async Task SelectFamilyRole(ITelegramBotClient botClient, Update update)
        {
            InlineCallback<EntityTCommand<FamilyRole>>? command = InlineCallback<EntityTCommand<FamilyRole>>
                 .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            var userChoice = command.Data.EntityId;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<AddToFamilyStepCache>();
            cache.FamilyRole = userChoice;

            await HandleSuccessfulAddConfirmationAsync(botClient, update, cache, handler);
        }

        private static async Task HandleSuccessfulAddConfirmationAsync(
            ITelegramBotClient botClient,
            Update update,
            AddToFamilyStepCache cache,
            StepTelegram handler)
        {
            await PRTelegramBot.Helpers.Message.Edit(
                botClient,
                update,
                FamilyConstants.AddTo.Messages.SuccessfulConfirmation(cache.FamilyRole));

            ConfirmationState<AddToFamilyStepCache>.SetUserResponse(update.CallbackQuery!.From!.Id, cache);

            StepFinalizer.FinalizeStep(update, handler);
        }
    }
}
