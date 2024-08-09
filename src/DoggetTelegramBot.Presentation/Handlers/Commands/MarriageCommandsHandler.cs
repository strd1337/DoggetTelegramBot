using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Presentation.Handlers.Common.Enums;
using DoggetTelegramBot.Presentation.Helpers.Common;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot;
using Telegram.Bot.Types;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;

namespace DoggetTelegramBot.Presentation.Handlers.Commands
{
    public class MarriageCommandsHandler
    {
        [InlineCallbackHandler<MarriageConfirmationCommands>(MarriageConfirmationCommands.Yes, MarriageConfirmationCommands.No)]
        public static async Task HandleUserAgreementResponse(ITelegramBotClient botClient, Update update)
        {
            long currentUser = update.CallbackQuery!.From!.Id;
            long necessaryUser = update.CallbackQuery.Message!.ReplyToMessage!.From!.Id;

            if (currentUser != necessaryUser)
            {
                await botClient.AnswerCallbackQueryAsync(
                    update.CallbackQuery.Id,
                    Constants.Messages.NotAllowed);

                return;
            }

            InlineCallback<EntityTCommand<bool>> command = InlineCallback<EntityTCommand<bool>>
                .GetCommandByCallbackOrNull(update.CallbackQuery.Data!);

            bool userChoice = command.Data.EntityId;
            ConfirmationState<bool>.SetUserResponse(currentUser, userChoice);

            await botClient.AnswerCallbackQueryAsync(
                update.CallbackQuery.Id,
                UserConstants.Requests.SendChoice(userChoice));
        }
    }
}
