using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Commands.Delete;
using DoggetTelegramBot.Application.Users.Queries.Get.Existence;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;
using DoggetTelegramBot.Infrastructure.BotManagement.Common.Handlers;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Infrastructure.BotManagement.Events
{
    public sealed class UpdateEventsHandler(
        IScopeService scopeService,
        IBotLogger logger)
    {
        public async Task<UpdateResult> HandleCheckUserExistance(
            ITelegramBotClient botClient,
            Update update)
        {
            logger.LogCommon(
                Constants.User.Messages.CheckExistenceRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            if (update.Message?.From is null && update.CallbackQuery?.From is null)
            {
                logger.LogCommon(
                     Constants.ErrorMessage.MissingInformation,
                     TelegramEvents.Message,
                     Constants.LogColors.PreUpdate);

                logger.LogCommon(
                    Constants.User.Messages.CheckExistenceRequest(false),
                    TelegramEvents.Message,
                    Constants.LogColors.Request);

                return UpdateResult.Stop;
            }

            if (update.Message?.From?.Id == botClient.BotId)
            {
                logger.LogCommon(
                   Constants.User.Messages.CheckExistenceRequest(false),
                   TelegramEvents.Message,
                   Constants.LogColors.Request);

                return UpdateResult.Stop;
            }

            CheckUserExistenceByTelegramIdQuery query;
            long userTelegramId;

            if (update.Message?.From is not null)
            {
                query = new CheckUserExistenceByTelegramIdQuery(
                    update.Message.From.Id,
                    update.Message.From.Username,
                    update.Message.From.FirstName);

                userTelegramId = update.Message.From.Id;
            }
            else
            {
                query = new CheckUserExistenceByTelegramIdQuery(
                    update.CallbackQuery!.From.Id,
                    update.CallbackQuery.From.Username,
                    update.CallbackQuery.From.FirstName);

                userTelegramId = update.CallbackQuery.From.Id;
            }

            var result = await scopeService.Send(query);

            logger.LogCommon(
                    Constants.User.Messages.SuccessExistence(userTelegramId),
                    TelegramEvents.Message,
                    Constants.LogColors.PreUpdate);

            logger.LogCommon(
                Constants.User.Messages.CheckExistenceRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return result.Value;
        }

        public async Task<UpdateResult> HandleAddChatMember(
            ITelegramBotClient botClient,
            Update update)
        {
            if (update.Message!.From!.Id == botClient.BotId)
            {
                return UpdateResult.Stop;
            }

            logger.LogCommon(
                Constants.User.Messages.AddNewChatMemberRequest(),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            OptionMessage options = new()
            {
                ReplyToMessageId = update.Message!.MessageId,
            };

            long? telegramId = update.Message?.From?.Id;

            if (telegramId is null)
            {
                await ErrorHandler.HandleMissingInformationError(
                    botClient,
                    update,
                    logger,
                    Constants.LogColors.Problem,
                    options);

                return UpdateResult.Continue;
            }

            long userTelegramId = telegramId.Value;
            decimal amount = Constants.Transaction.Costs.NewChatMemberReward;
            string successMessage = Constants.User.Messages.RewardSentSuccessfully(
                amount,
                RewardType.NewChatMember);

            await RewardHandler.RewardUserAsync(
                botClient,
                update,
                scopeService,
                logger,
                options,
                amount,
                userTelegramId,
                successMessage);

            logger.LogCommon(
                Constants.User.Messages.AddNewChatMemberRequest(false),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            return UpdateResult.Continue;
        }

        public async Task<UpdateResult> HandleChatMemberLeft(
            ITelegramBotClient botClient,
            Update update)
        {
            if (update.Message!.From!.Id == botClient.BotId)
            {
                return UpdateResult.Stop;
            }

            logger.LogCommon(
                Constants.User.Messages.ChatMemberLeftRequest(),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            long? telegramId = update.Message?.From?.Id;

            if (telegramId is null)
            {
                logger.LogCommon(
                    Constants.ErrorMessage.MissingInformation,
                    TelegramEvents.GroupAction,
                    Constants.LogColors.Problem);

                return UpdateResult.Continue;
            }

            logger.LogCommon(
                Constants.User.Messages.DeleteRequest(),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            long userTelegramId = telegramId.Value;

            DeleteUserByTelegramIdCommand command = new(userTelegramId);

            var result = await scopeService.Send(command);

            logger.LogCommon(
                Constants.User.Messages.DeleteRequest(false),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            logger.LogCommon(
                Constants.User.Messages.ChatMemberLeftRequest(false),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            return result.IsError ? UpdateResult.Stop : UpdateResult.Continue;
        }
    }
}
