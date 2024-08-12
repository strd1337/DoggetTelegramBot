namespace DoggetTelegramBot.Application.Helpers.CacheKeys
{
    public static class CommandCacheKeyGenerator
    {
        private const string ModelName = "Command";

        public static string CommandUsageByTelegramId(long telegramId, string commandName) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(CommandUsageByTelegramId), $"{commandName}-{telegramId}");
    }

}
