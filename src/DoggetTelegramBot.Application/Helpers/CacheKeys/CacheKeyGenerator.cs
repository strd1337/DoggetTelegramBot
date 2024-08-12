namespace DoggetTelegramBot.Application.Helpers.CacheKeys
{
    public static class CacheKeyGenerator
    {
        private const string Separator = "-";

        public static string GenerateKey(
            string modelName,
            string methodName,
            string? keyComponent = null) =>
                string.IsNullOrEmpty(keyComponent) ?
                $"{modelName}{Separator}{methodName}" :
                $"{modelName}{Separator}{methodName}{Separator}{keyComponent}";

        public static string GenerateKey(
            string modelName,
            string methodName,
            long keyComponent) =>
                GenerateKey(modelName, methodName, $"{keyComponent}");

        public static string GenerateKey(
            string modelName,
            string methodName,
            IEnumerable<long> keyComponents) =>
                GenerateKey(modelName, methodName, string.Join(",", keyComponents));
    }
}
