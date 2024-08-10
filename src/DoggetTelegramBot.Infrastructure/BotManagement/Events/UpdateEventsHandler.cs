using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Commands.Delete;
using DoggetTelegramBot.Application.Users.Queries.Get.Existence;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;
using DoggetTelegramBot.Infrastructure.BotManagement.Common.Handlers;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using TransactionConstants = DoggetTelegramBot.Domain.Common.Constants.Transaction.Constants.Transaction;
using ErrorConstants = DoggetTelegramBot.Domain.Common.Constants.Error.Constants.Errors;

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
                UserConstants.Requests.CheckExistence(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            if (update.Message?.From is null && update.CallbackQuery?.From is null)
            {
                logger.LogCommon(
                     ErrorConstants.Messages.MissingInformation,
                     TelegramEvents.Message,
                     LoggerConstants.Colors.PreUpdate);

                logger.LogCommon(
                    UserConstants.Requests.CheckExistence(false),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.Request);

                return UpdateResult.Stop;
            }
            //TO DO: REMOVE
            if (update.Message?.From?.Id == botClient.BotId)
            {
                logger.LogCommon(
                   UserConstants.Requests.CheckExistence(false),
                   TelegramEvents.Message,
                   LoggerConstants.Colors.Request);

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
                UserConstants.Logging.SuccessExistence(userTelegramId),
                TelegramEvents.Message,
                LoggerConstants.Colors.PreUpdate);

            logger.LogCommon(
                UserConstants.Requests.CheckExistence(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

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
                UserConstants.Requests.AddNewChatMember(),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

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
                    LoggerConstants.Colors.Problem,
                    options);

                return UpdateResult.Continue;
            }

            long userTelegramId = telegramId.Value;
            decimal amount = TransactionConstants.Costs.NewChatMemberReward;
            string successMessage = UserConstants.Messages.RewardSentSuccessfully(
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
                UserConstants.Requests.AddNewChatMember(false),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

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
                UserConstants.Requests.ChatMemberLeft(),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            long? telegramId = update.Message?.From?.Id;

            if (telegramId is null)
            {
                logger.LogCommon(
                    ErrorConstants.Messages.MissingInformation,
                    TelegramEvents.GroupAction,
                    LoggerConstants.Colors.Problem);

                return UpdateResult.Continue;
            }

            logger.LogCommon(
                UserConstants.Requests.Delete(),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            long userTelegramId = telegramId.Value;

            DeleteUserByTelegramIdCommand command = new(userTelegramId);

            var result = await scopeService.Send(command);

            logger.LogCommon(
                UserConstants.Requests.Delete(false),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            logger.LogCommon(
                UserConstants.Requests.ChatMemberLeft(false),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            return result.IsError ? UpdateResult.Stop : UpdateResult.Continue;
        }
    }
}
