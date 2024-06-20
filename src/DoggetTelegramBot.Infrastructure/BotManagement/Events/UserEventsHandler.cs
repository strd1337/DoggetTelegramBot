using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Queries.Get.Existence;
using DoggetTelegramBot.Application.Users.Queries.Get.Privileges;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Helpers = PRTelegramBot.Helpers;

namespace DoggetTelegramBot.Infrastructure.BotManagement.Events
{
    public sealed class UserEventsHandler(
        IScopeService service,
        IBotLogger logger)
    {
        public async Task HandleCheckPrivilege(
            ITelegramBotClient botclient,
            Update update,
            Func<ITelegramBotClient, Update, Task> callback,
            int? flags = null)
        {
            logger.LogCommon(
                Constants.User.Messages.CheckPrivilegeRequest(),
                TelegramEvents.Message);

            long? telegramId = update.Message?.From?.Id;

            if (telegramId is null || flags is null)
            {
                logger.LogCommon(
                    Constants.ErrorMessage.MissingInformation,
                    TelegramEvents.Message);

                await SendMessage(
                        botclient,
                        update,
                        Constants.ErrorMessage.MissingInformation);

                return;
            }

            GetUserPrivilegesByTelegramIdQuery query = new(telegramId.Value);
            var result = await service.Send(query);

            if (!result.IsError)
            {
                var privileges = result.Value.UserPrivileges;

                if (HasRequiredPrivilege(privileges, (UserPrivilege)flags))
                {
                    logger.LogCommon(
                       Constants.User.Messages.AccessedSuccessfully(telegramId.Value),
                       TelegramEvents.Message);

                    await callback(botclient, update);
                }
                else
                {
                    logger.LogCommon(
                       Constants.User.Messages.FailedAccess(telegramId.Value),
                       TelegramEvents.Message);

                    await SendMessage(
                        botclient,
                        update,
                        Constants.ErrorMessage.NotAllowedFunction);
                }
            }
            else
            {
                await SendMessage(
                    botclient,
                    update,
                    Constants.ErrorMessage.NotAllowedFunction);
            }

            logger.LogCommon(
                Constants.User.Messages.CheckPrivilegeRequest(false),
                TelegramEvents.Message);
        }

        public async Task<ResultUpdate> HandleCheckUserExistance(
            ITelegramBotClient botclient,
            Update update)
        {
            logger.LogCommon(
                Constants.User.Messages.CheckExistenceRequest(),
                TelegramEvents.Message);

            if (update.Message?.From is null)
            {
                logger.LogCommon(
                    Constants.ErrorMessage.MissingInformation,
                    TelegramEvents.Message);

                await SendMessage(
                        botclient,
                        update,
                        Constants.ErrorMessage.MissingInformation);

                return ResultUpdate.Stop;
            }

            CheckUserExistenceByTelegramIdQuery query = new(
                update.Message.From.Id,
                update.Message.From.Username);

            var result = await service.Send(query);

            logger.LogCommon(
                Constants.User.Messages.SuccessExistence(update.Message.From.Id),
                TelegramEvents.Message);

            logger.LogCommon(
                Constants.User.Messages.CheckExistenceRequest(false),
                TelegramEvents.Message);

            return result.Value;
        }

        private static bool HasRequiredPrivilege(
            IEnumerable<UserPrivilege> userPrivileges,
            UserPrivilege requiredPrivileges) =>
                userPrivileges.Any(p => requiredPrivileges.HasFlag(p));

        private static async Task SendMessage(
            ITelegramBotClient botclient,
            Update update,
            string errorMessage) =>
                await Helpers.Message.Send(botclient, update, errorMessage);
    }
}
