using DoggetTelegramBot.Application.Common.Services;
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
        public async Task GetMyInfo(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(
                Constants.User.Messages.GetInformationRequest(),
                TelegramEvents.Message);

            GetUserInfoQuery query = new(update.Message!.From!.Id);
            var result = await service.Send(query);

            var response = result.Match(mapper.Map<Response>, Problem);

            await Helpers.Message.Send(botClient, update, response.Message);

            logger.LogCommon(
                Constants.User.Messages.GetInformationRequest(false),
                TelegramEvents.Message);
        }
    }
}
