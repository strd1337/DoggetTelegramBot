using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Inventories.Commands.Transfer;
using DoggetTelegramBot.Application.Inventories.Queries.Get.Info;
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
    public sealed class InventoryController(
        IBotLogger logger,
        IScopeService service,
        IMapper mapper,
        ITelegramBotService botService) : BaseController(botService)
    {
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

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
               Constants.Inventory.Messages.GetInformationRequest(false),
               TelegramEvents.Message,
               Constants.LogColors.Request);
        }

        [ReplyMenuHandler(CommandComparison.Contains, StringComparison.OrdinalIgnoreCase, Constants.Inventory.ReplyKeys.Transfer)]
        public async Task Transfer(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Inventory.Messages.TransferRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            string text = update.Message!.Text!
                [Constants.Inventory.ReplyKeys.Transfer.Length..]
                .Trim();

            if (update.Message?.ReplyToMessage is null ||
                !decimal.TryParse(text, out decimal amount))
            {
                await SendReplyMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Constants.Inventory.ReplyKeys.Transfer,
                        Constants.Inventory.ReplyKeys.TransferKey));

                return;
            }

            TransferMoneyCommand command = new(
                update.Message.From!.Id,
                update.Message.ReplyToMessage.From!.Id,
                amount);

            var result = await service.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
               Constants.Inventory.Messages.TransferRequest(false),
               TelegramEvents.Message,
               Constants.LogColors.Request);
        }
    }
}
