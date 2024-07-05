using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Inventories.Queries.Get.Info;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Mapping;
using MapsterMapper;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public sealed class InventoryController(
        IBotLogger logger,
        IScopeService service,
        IMapper mapper) : BaseController(logger)
    {
        private readonly IBotLogger logger = logger;

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.Inventory.ReplyKeys.GetInfo)]
        public async Task GetInfo(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Inventory.Messages.GetInformationRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            GetInventoryInfoByTelegramIdQuery query = new(update.Message!.From!.Id);
            var result = await service.Send(query);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendMessage(botClient, update, response.Message);

            logger.LogCommon(
               Constants.Inventory.Messages.GetInformationRequest(false),
               TelegramEvents.Message,
               Constants.LogColors.Request);
        }
    }
}
