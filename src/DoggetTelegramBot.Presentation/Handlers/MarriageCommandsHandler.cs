using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.Common.Services;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.Handlers
{
    public class MarriageCommandsHandler
    {
        [InlineCallbackHandler<UserResponse>(UserResponse.Yes, UserResponse.No)]
        public static async Task HandleUserAgreementResponse(ITelegramBotClient botClient, Update update)
        {
            InlineCallback<EntityTCommand<bool>>? command = InlineCallback<EntityTCommand<bool>>
                .GetCommandByCallbackOrNull(update.CallbackQuery!.Data!);

            if (command is not null)
            {
                UserState.SetUserResponse(
                    update.CallbackQuery.Message!.From!.Id,
                    command.Data.EntityId);

                if (update.CallbackQuery.From.Id == update.CallbackQuery.Message.ReplyToMessage!.From!.Id)
                {
                    await botClient.AnswerCallbackQueryAsync(
                        update.CallbackQuery.Id,
                        Constants.User.Messages.SendChoice(command.Data.EntityId));
                }
                else
                {
                    await botClient.AnswerCallbackQueryAsync(
                        update.CallbackQuery.Id,
                        Constants.Messages.NotAllowed);
                }
            }
        }
    }
}
