using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Queries.Get;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Helpers = PRTelegramBot.Helpers;

namespace DoggetTelegramBot.Infrastructure.BotManagement
{
    public sealed class BotEvents(IScopeService service)
    {
        public async Task OnCheckPrivilege(
            ITelegramBotClient botclient,
            Update update,
            Func<ITelegramBotClient, Update, Task> callback,
            int? flags = null)
        {
            if (update.Message?.From?.Id is null || flags is null)
            {
                await SendMessage(
                        botclient,
                        update,
                        Constants.ErrorMessage.MissingPrivilegeInformation);

                return;
            }

            long telegramId = update.Message.From.Id;
            GetUserPrivilegesQuery query = new(telegramId);

            var result = await service.Send(query);

            if (!result.IsError)
            {
                var privileges = result.Value.UserPrivileges;

                if (HasRequiredPrivilege(privileges, (UserPrivilege)flags))
                {
                    await callback(botclient, update);
                }
                else
                {
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
