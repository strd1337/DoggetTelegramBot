using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Queries.Get.Privileges;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using DoggetTelegramBot.Infrastructure.BotManagement.Common.Handlers;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Helpers = PRTelegramBot.Helpers;

namespace DoggetTelegramBot.Infrastructure.BotManagement.Events
{
    public sealed class CommonEventsHandler(
        IScopeService service,
        IBotLogger logger)
    {
        public async Task HandleCheckPrivilege(
            ITelegramBotClient botClient,
            Update update,
            Func<ITelegramBotClient, Update, Task> callback,
            int? flags = null)
        {
            logger.LogCommon(
                Constants.User.Messages.CheckPrivilegeRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            long? telegramId = update.Message?.From?.Id;

            if (telegramId is null || flags is null)
            {
                OptionMessage options = new()
                {
                    ReplyToMessageId = update.Message!.MessageId,
                };

                await ErrorHandler.HandleMissingInformationError(
                    botClient,
                    update,
                    logger,
                    Constants.LogColors.CheckPrivileges,
                    options);

                return;
            }

            GetUserPrivilegesByTelegramIdQuery query = new(telegramId.Value);
            var result = await service.Send(query);

            if (!result.IsError)
            {
                var privileges = result.Value.UserPrivileges;

                if (PrivilegeHelper.HasRequiredPrivilege(privileges, (UserPrivilege)flags))
                {
                    logger.LogCommon(
                       Constants.User.Messages.AccessedSuccessfully(telegramId.Value),
                       TelegramEvents.Message,
                       Constants.LogColors.CheckPrivileges);

                    await callback(botClient, update);
                }
                else
                {
                    logger.LogCommon(
                      Constants.User.Messages.FailedAccess(telegramId.Value),
                      TelegramEvents.Message,
                      Constants.LogColors.CheckPrivileges);

                    await Helpers.Message.Send(
                       botClient,
                       update,
                       Constants.ErrorMessage.NotAllowedFunction);
                }
            }
            else
            {
                await Helpers.Message.Send(
                      botClient,
                      update,
                      Constants.ErrorMessage.NotAllowedFunction);
            }

            logger.LogCommon(
               Constants.User.Messages.CheckPrivilegeRequest(false),
               TelegramEvents.Message,
               Constants.LogColors.Request);
        }
    }
}
