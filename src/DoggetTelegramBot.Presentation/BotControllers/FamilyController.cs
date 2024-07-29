using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Services;
using DoggetTelegramBot.Presentation.Handlers.Common.Caches;
using DoggetTelegramBot.Presentation.Handlers.Requests;
using DoggetTelegramBot.Presentation.Handlers.StepCommands;
using DoggetTelegramBot.Presentation.Helpers.MenuGenerators;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public sealed class FamilyController(
        IBotLogger logger,
        ITelegramBotService botService,
        FamilyRequestHandler requestHandler) : BaseController(botService)
    {
        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Constants.Family.ReplyKeys.AddToFamily)]
        public async Task AddAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                Constants.Family.Messages.AddToFamilyRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            if (update.Message?.ReplyToMessage is null)
            {
                await SendReplyMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Constants.Family.ReplyKeys.AddToFamily));

                return;
            }

            AddToFamilyStepCache cache = new();

            StepTelegram handler = new(
                AddToFamilyStepCommandsHandler.SelectFamilyRole,
                cache);

            update.RegisterStepHandler(handler);

            var selectFamilyRoleMenu = AddToFamilyMenuGenerator.GenerateFamilyRoleSelectionMenu();

            var message = await SendMessage(
                botClient,
                update,
                Constants.Family.Messages.SelectFamilyRoleRequest,
                selectFamilyRoleMenu);

            _ = requestHandler.HandleAddToFamilyAsync(botClient, update, message);
        }
    }
}
