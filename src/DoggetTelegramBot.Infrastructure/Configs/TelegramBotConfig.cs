namespace DoggetTelegramBot.Infrastructure.Configs
{
    public sealed class TelegramBotConfig
    {
        public int BotId { get; set; }
        public string Token { get; set; } = default!;
        public bool ShowErrorNotFoundNameButton { get; set; }
        public bool ClearUpdatesOnStart { get; set; }
    }
}
