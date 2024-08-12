using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Commands.Remove;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Presentation.BotCommands;
using DoggetTelegramBot.Presentation.BotControllers.Common;
using DoggetTelegramBot.Presentation.Common.Mapping;
using DoggetTelegramBot.Presentation.Common.Services;
using DoggetTelegramBot.Presentation.Handlers.Common.Caches;
using DoggetTelegramBot.Presentation.Handlers.Requests;
using DoggetTelegramBot.Presentation.Handlers.StepCommands;
using DoggetTelegramBot.Presentation.Helpers.MenuGenerators;
using MapsterMapper;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using FamilyConstants = DoggetTelegramBot.Domain.Common.Constants.Family.Constants.Family;

namespace DoggetTelegramBot.Presentation.BotControllers
{
    [BotHandler]
    public sealed class FamilyController(
        IBotLogger logger,
        FamilyRequestHandler requestHandler,
        IScopeService scopeService,
        IMapper mapper,
        ITelegramBotService botService) : BaseController(botService)
    {
        private readonly ITelegramBotService botService = botService;

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.Family.AddToFamily)]
        public async Task AddAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                FamilyConstants.Requests.AddToFamily(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            if (update.Message?.ReplyToMessage is null)
            {
                await SendReplyMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Commands.Family.AddToFamily));

                logger.LogCommon(
                    FamilyConstants.Requests.AddToFamily(false),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.Request);

                return;
            }

            AddToFamilyStepCache cache = new();

            StepTelegram handler = new(
                AddToFamilyStepCommandsHandler.SelectFamilyRole,
                cache);

            update.RegisterStepHandler(handler);

            var selectFamilyRoleMenu = AddToFamilyMenuGenerator.GenerateFamilyRoleSelectionMenu(update);

            var message = await SendMessage(
                botClient,
                update,
                FamilyConstants.AddTo.Messages.SelectFamilyRoleRequest,
                selectFamilyRoleMenu);

            _ = requestHandler.HandleAddToFamilyAsync(botClient, update, message);
        }

        [ReplyMenuHandler(CommandComparison.Equals, StringComparison.OrdinalIgnoreCase, Commands.Family.RemoveFromFamily)]
        public async Task RemoveAsync(ITelegramBotClient botClient, Update update)
        {
            logger.LogCommon(update);

            logger.LogCommon(
                FamilyConstants.Requests.RemoveFromFamily(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            if (update.Message?.ReplyToMessage is null)
            {
                await SendReplyMessage(
                    botClient,
                    update,
                    Constants.Messages.NotFoundUserReply(
                        Commands.Family.RemoveFromFamily));
            }
            else
            {
                long parentTelegramId = update.Message!.From!.Id;
                long memberToRemoveTelegramId = update.Message.ReplyToMessage!.From!.Id;

                RemoveFromFamilyCommand command = new(
                    parentTelegramId,
                    memberToRemoveTelegramId);

                var result = await scopeService.Send(command);

                var response = result.Match(mapper.Map<Response>, botService.Problem);

                await botService.SendMessage(
                    botClient,
                    update,
                    response.Message);
            }

            logger.LogCommon(
                FamilyConstants.Requests.RemoveFromFamily(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);
        }
    }
}
