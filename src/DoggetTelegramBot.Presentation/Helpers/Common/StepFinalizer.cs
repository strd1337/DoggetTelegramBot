using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using Telegram.Bot.Types;

namespace DoggetTelegramBot.Presentation.Helpers.Common
{
    public static class StepFinalizer
    {
        public static void FinalizeStep(Update update, StepTelegram handler)
        {
            handler.LastStepExecuted = true;
            update.ClearStepUserHandler();
        }
    }
}
