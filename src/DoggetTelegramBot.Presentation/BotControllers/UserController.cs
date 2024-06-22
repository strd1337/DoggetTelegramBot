using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Commands.Update.Nickname;
using DoggetTelegramBot.Application.Users.Queries.Get.Information;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Mapping;
using MapsterMapper;
using PRTelegramBot.Attributes;
using Telegram.Bot;
using Telegram.Bot.Types;
using Helpers = PRTelegramBot.Helpers;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    public class UserController(
        IBotLogger logger,
        IScopeService service,
        IMapper mapper) : BaseController(logger)
    {
        private readonly IBotLogger logger = logger;

        [ReplyMenuHandler(Constants.User.ReplyKeys.GetMyInfo)]
        public async Task GetInfo(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(
                Constants.User.Messages.GetInformationRequest(),
                TelegramEvents.Message);

            GetUserInfoByTelegramIdQuery query = new(update.Message!.From!.Id);
            var result = await service.Send(query);

            var response = result.Match(mapper.Map<Response>, Problem);

            await Helpers.Message.Send(botClient, update, response.Message);

            logger.LogCommon(
                Constants.User.Messages.GetInformationRequest(false),
                TelegramEvents.Message);
        }

        [ReplyMenuHandler(Constants.User.ReplyKeys.UpdateNickname)]
        public async Task UpdateNickname(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(
                Constants.User.Messages.UpdateNicknameRequest(),
                TelegramEvents.Message);

            // TO DO: change nickname
            UpdateNicknameByTelegramIdCommand command = new(
                update.Message!.From!.Id,
                "remove");

            var result = await service.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await Helpers.Message.Send(botClient, update, response.Message);

            logger.LogCommon(
                Constants.User.Messages.UpdateNicknameRequest(false),
                TelegramEvents.Message);
        }

        [ReplyMenuHandler(Constants.User.ReplyKeys.DeleteNickname)]
        public async Task DeleteNickname(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(
               Constants.User.Messages.UpdateNicknameRequest(),
               TelegramEvents.Message);

            UpdateNicknameByTelegramIdCommand command = new(
                update.Message!.From!.Id,
                null);

            var result = await service.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await Helpers.Message.Send(botClient, update, response.Message);

            logger.LogCommon(
                Constants.User.Messages.UpdateNicknameRequest(false),
                TelegramEvents.Message);
        }
    }
}
