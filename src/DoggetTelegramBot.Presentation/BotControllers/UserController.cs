using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Commands.Update.Nickname;
using DoggetTelegramBot.Application.Users.Queries.Get.Information;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.BotCommands;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Mapping;
using DoggetTelegramBot.Presentation.Common.Services;
using MapsterMapper;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public class UserController(
        IBotLogger logger,
        IScopeService service,
        IMapper mapper,
        ITelegramBotService botService) : BaseController(botService)
    {
        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.User.GetMyInfo)]
        public async Task GetInfoAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                UserConstants.Requests.GetInformation(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetUserInfoByTelegramIdQuery query = new(update.Message!.From!.Id);
            var result = await service.Send(query);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
                UserConstants.Requests.GetInformation(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);
        }

        [ReplyMenuHandler(CommandComparison.Contains, StringComparison.OrdinalIgnoreCase, Commands.User.UpdateNickname)]
        public async Task UpdateNicknameAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                UserConstants.Requests.UpdateNickname(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            string nickname = update.Message!.Text!
                [Commands.User.UpdateNickname.Length..]
                .Trim();

            UpdateNicknameByTelegramIdCommand command = new(
                update.Message!.From!.Id,
                nickname);

            var result = await service.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
               UserConstants.Requests.UpdateNickname(false),
               TelegramEvents.Message,
               LoggerConstants.Colors.Request);
        }

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.User.DeleteNickname)]
        public async Task DeleteNicknameAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                UserConstants.Requests.UpdateNickname(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            UpdateNicknameByTelegramIdCommand command = new(
                update.Message!.From!.Id,
                null);

            var result = await service.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
                UserConstants.Requests.UpdateNickname(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);
        }
    }
}
