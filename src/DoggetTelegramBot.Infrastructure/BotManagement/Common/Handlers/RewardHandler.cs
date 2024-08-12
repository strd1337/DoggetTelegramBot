using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Transactions.Commands;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Helpers = PRTelegramBot.Helpers;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;

namespace DoggetTelegramBot.Infrastructure.BotManagement.Common.Handlers
{
    public static class RewardHandler
    {
        public static async Task RewardUserAsync(
            ITelegramBotClient botClient,
            Update update,
            IScopeService scopeService,
            IBotLogger logger,
            OptionMessage options,
            decimal amount,
            long userTelegramId,
            string successfulMessage)
        {
            bool isSuccess = await RewardUserAsync(
                botClient, update, scopeService, logger, options, amount, userTelegramId);

            if (isSuccess)
            {
                await Helpers.Message.Send(
                    botClient,
                    update,
                    successfulMessage,
                    options);
            }
        }

        public static async Task<bool> RewardUserAsync(
            ITelegramBotClient botClient,
            Update update,
            IScopeService scopeService,
            IBotLogger logger,
            OptionMessage options,
            decimal amount,
            long userTelegramId)
        {
            RewardTransactionCommand command = new([userTelegramId], amount);

            var transactionResult = await scopeService.Send(command);

            if (transactionResult.IsError)
            {
                await ErrorHandler.HandleTransactionError(
                    botClient,
                    update,
                    options,
                    transactionResult.Errors.First(),
                    logger,
                    LoggerConstants.Colors.Problem);

                return false;
            }

            return true;
        }
    }
}
