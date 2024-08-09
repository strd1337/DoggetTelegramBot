using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Queries.Get.Privileges;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using DoggetTelegramBot.Infrastructure.BotManagement.Common.Handlers;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Helpers = PRTelegramBot.Helpers;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using ErrorConstants = DoggetTelegramBot.Domain.Common.Constants.Error.Constants.Errors;

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
                UserConstants.Requests.CheckPrivilege(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

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
                    LoggerConstants.Colors.CheckPrivileges,
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
                       UserConstants.Logging.AccessedSuccessfully(telegramId.Value),
                       TelegramEvents.Message,
                       LoggerConstants.Colors.CheckPrivileges);

                    await callback(botClient, update);
                }
                else
                {
                    logger.LogCommon(
                      UserConstants.Logging.FailedAccess(telegramId.Value),
                      TelegramEvents.Message,
                      LoggerConstants.Colors.CheckPrivileges);

                    await Helpers.Message.Send(
                       botClient,
                       update,
                       ErrorConstants.Messages.NotAllowedFunction);
                }
            }
            else
            {
                await Helpers.Message.Send(
                      botClient,
                      update,
                      ErrorConstants.Messages.NotAllowedFunction);
            }

            logger.LogCommon(
               UserConstants.Requests.CheckPrivilege(false),
               TelegramEvents.Message,
               LoggerConstants.Colors.Request);
        }
    }
}
