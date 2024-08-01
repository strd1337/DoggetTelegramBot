using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Dungeons.Guimu.Commands;
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
    public sealed class DungeonController(
        IBotLogger logger,
        IScopeService scopeService,
        IMapper mapper,
        ITelegramBotService botService) : BaseController(botService)
    {
        [SlashHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.Dungeon.ReplyKeys.Guimu)]
        public async Task StartGuimuAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Dungeon.Guimu.Messages.Start(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            GuimuDungeonCommand command = new(update.Message!.From!.Id);
            var result = await scopeService.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
                Constants.Dungeon.Guimu.Messages.Start(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);
        }
    }
}
