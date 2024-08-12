using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Inventories.Commands.Transfer;
using DoggetTelegramBot.Application.Inventories.Queries.Get.Info;
using DoggetTelegramBot.Domain.Common.Constants;
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
using InventoryConstants = DoggetTelegramBot.Domain.Common.Constants.Inventory.Constants.Inventory;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public sealed class InventoryController(
        IBotLogger logger,
        IScopeService service,
        IMapper mapper,
        ITelegramBotService botService) : BaseController(botService)
    {
        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.Inventory.GetInfo)]
        public async Task GetInfoAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                InventoryConstants.Requests.GetInformation(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetInventoryInfoByTelegramIdQuery query = new(update.Message!.From!.Id);
            var result = await service.Send(query);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
               InventoryConstants.Requests.GetInformation(false),
               TelegramEvents.Message,
               LoggerConstants.Colors.Request);
        }

        [ReplyMenuHandler(CommandComparison.Contains, StringComparison.OrdinalIgnoreCase, Commands.Inventory.Transfer)]
        public async Task TransferAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                InventoryConstants.Requests.Transfer(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            string text = update.Message!.Text!
                [Commands.Inventory.Transfer.Length..]
                .Trim();

            if (update.Message?.ReplyToMessage is null ||
                !decimal.TryParse(text, out decimal amount))
            {
                await SendReplyMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Commands.Inventory.Transfer,
                        Commands.Inventory.TransferKey));
            }
            else
            {
                TransferMoneyCommand command = new(
                    update.Message.From!.Id,
                    update.Message.ReplyToMessage.From!.Id,
                    amount);

                var result = await service.Send(command);

                var response = result.Match(mapper.Map<Response>, Problem);

                await SendReplyMessage(botClient, update, response.Message);
            }

            logger.LogCommon(
               InventoryConstants.Requests.Transfer(false),
               TelegramEvents.Message,
               LoggerConstants.Colors.Request);
        }
    }
}
