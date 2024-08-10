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
using MarriageConstants = DoggetTelegramBot.Domain.Common.Constants.Marriage.Constants.Marriage;
using DoggetTelegramBot.Presentation.Helpers.Common;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;

namespace DoggetTelegramBot.Presentation.Handlers.StepCommands
{
    public sealed class MarryStepCommandsHandler
    {
        [InlineCallbackHandler<MarryCommands>(MarryCommands.SelectConfirmationType)]
        public static async Task SelectConfirmationType(ITelegramBotClient botClient, Update update)
        {
            if (!await CallbackQueryHelper.IsUserAllowedAsync(botClient, update))
            {
                return;
            }

            InlineCallback<EntityTCommand<MarriageConfirmationCommands>>? command = InlineCallback<EntityTCommand<MarriageConfirmationCommands>>
                 .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            var userChoice = command.Data.EntityId;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<MarryStepCache>();
            cache.ConfirmationCommand = userChoice;

            if (cache.ConfirmationCommand is MarriageConfirmationCommands.No)
            {
                await HandleMarryConfirmationAsync(botClient, update, cache, handler);
                return;
            }

            handler.RegisterNextStep(SelectMarriageType);

            var confirmationMenu = MarryMenuGenerator.GenerateMarriageTypeMenu(update);

            await PRTelegramBot.Helpers.Message.Edit(
                botClient,
                update,
                MarriageConstants.Marry.Messages.SelectMarriageTypeRequest,
                confirmationMenu);
        }

        [InlineCallbackHandler<MarryCommands>(MarryCommands.SelectMarriageType)]
        public static async Task SelectMarriageType(ITelegramBotClient botClient, Update update)
        {
            if (!await CallbackQueryHelper.IsUserAllowedAsync(botClient, update))
            {
                return;
            }

            InlineCallback<EntityTCommand<MarriageType>>? command = InlineCallback<EntityTCommand<MarriageType>>
                 .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            var userChoice = command.Data.EntityId;

            var handler = update.GetStepHandler<StepTelegram>();

            var cache = handler!.GetCache<MarryStepCache>();
            cache.MarriageType = userChoice;

            await HandleMarryConfirmationAsync(botClient, update, cache, handler);
        }

        private static async Task HandleMarryConfirmationAsync(
            ITelegramBotClient botClient,
            Update update,
            MarryStepCache cache,
            StepTelegram handler)
        {
            if (cache.ConfirmationCommand is MarriageConfirmationCommands.Yes)
            {
                await PRTelegramBot.Helpers.Message.Edit(
                    botClient,
                    update,
                    MarriageConstants.Marry.Messages.SuccessfulConfirmation(cache.MarriageType));
            }

            ConfirmationState<MarryStepCache>.SetUserResponse(update.CallbackQuery!.From!.Id, cache);

            StepFinalizer.FinalizeStep(update, handler);
        }
    }
}
