using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Transactions.Commands;
using DoggetTelegramBot.Domain.Common.Constants;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Helpers = PRTelegramBot.Helpers;

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
                    Constants.LogColors.Problem);
            }
            else
            {
                await Helpers.Message.Send(
                    botClient,
                    update,
                    successfulMessage,
                    options);
            }
        }
    }
}
