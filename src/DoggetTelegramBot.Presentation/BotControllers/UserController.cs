using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Commands.Update.Nickname;
using DoggetTelegramBot.Application.Users.Queries.Get.Information;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Mapping;
using DoggetTelegramBot.Presentation.Common.Services;
using MapsterMapper;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public class UserController(
        IBotLogger logger,
        IScopeService service,
        IMapper mapper,
        ITelegramBotService botService) : BaseController(botService)
    {
        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.User.ReplyKeys.GetMyInfo)]
        public async Task GetInfo(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.User.Messages.GetInformationRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            GetUserInfoByTelegramIdQuery query = new(update.Message!.From!.Id);
            var result = await service.Send(query);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
                Constants.User.Messages.GetInformationRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);
        }

        [ReplyMenuHandler(CommandComparison.Contains, StringComparison.OrdinalIgnoreCase, Constants.User.ReplyKeys.UpdateNickname)]
        public async Task UpdateNickname(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.User.Messages.UpdateNicknameRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            string nickname = update.Message!.Text!
                [Constants.User.ReplyKeys.UpdateNickname.Length..]
                .Trim();

            UpdateNicknameByTelegramIdCommand command = new(
                update.Message!.From!.Id,
                nickname);

            var result = await service.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
               Constants.User.Messages.UpdateNicknameRequest(false),
               TelegramEvents.Message,
               Constants.LogColors.Request);
        }

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.User.ReplyKeys.DeleteNickname)]
        public async Task DeleteNickname(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.User.Messages.UpdateNicknameRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            UpdateNicknameByTelegramIdCommand command = new(
                update.Message!.From!.Id,
                null);

            var result = await service.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
                Constants.User.Messages.UpdateNicknameRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);
        }
    }
}
