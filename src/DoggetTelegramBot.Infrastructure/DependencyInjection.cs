using DoggetTelegramBot.Infrastructure.Configs;
using DoggetTelegramBot.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PRTelegramBot.Extensions;

namespace DoggetTelegramBot.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTelegramBot(configuration);

            return services;
        }

        public static IServiceCollection AddTelegramBot(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddBotHandlers();

            services.AddOptions();
            services.Configure<TelegramBotConfig>(configuration.GetSection(TelegramBotConfig.OptionKey));

            services.AddSingleton<TelegramBotInitializer>();

            return services;
        }
    }
}
