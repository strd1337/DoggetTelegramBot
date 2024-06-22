using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Commands.Create;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
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
    public class MarriageController(
        IBotLogger logger,
        IScopeService service,
        IMapper mapper) : BaseController(logger)
    {
        private readonly IBotLogger logger = logger;

        [ReplyMenuHandler(CommandComparison.Contains, StringComparison.OrdinalIgnoreCase, Constants.Marriage.ReplyKeys.CreateMarriage)]
        public async Task Create(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Marriage.Messages.CreateMarriageRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            // TO DO: change the data
            List<long> spouses = [442632563, 905753288];

            CreateMarriageCommand command = new(
                MarriageType.Civil,
                spouses);

            var result = await service.Send(command);

            var response = result.Match(mapper.Map<Response>, Problem);

            await SendMessage(botClient, update, response.Message);

            logger.LogCommon(
                Constants.Marriage.Messages.CreateMarriageRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);
        }
    }
}
