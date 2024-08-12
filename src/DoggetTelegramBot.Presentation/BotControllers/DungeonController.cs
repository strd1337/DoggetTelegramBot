using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Dungeons.Guimu.Commands;
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
using DungeonConstants = DoggetTelegramBot.Domain.Common.Constants.Dungeon.Guimu.Constants.Dungeon;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public sealed class DungeonController(
        IBotLogger logger,
        IScopeService scopeService,
        IMapper mapper,
        ITelegramBotService botService) : BaseController(botService)
    {
        [SlashHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.Dungeon.Guimu)]
        public async Task StartGuimuAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                DungeonConstants.Guimu.Messages.Start(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GuimuDungeonCommand command = new(update.Message!.From!.Id);
            var result = await scopeService.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendReplyMessage(botClient, update, response.Message);

            logger.LogCommon(
                DungeonConstants.Guimu.Messages.Start(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);
        }
    }
}
