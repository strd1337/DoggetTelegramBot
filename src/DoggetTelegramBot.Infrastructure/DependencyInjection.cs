using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;
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

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            NLogConfigurate.Configurate();

            return services;
        }

        public static IServiceCollection AddTelegramBot(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddBotHandlers();

            services.AddOptions();
            services.Configure<TelegramBotConfig>(configuration.GetSection(Constants.TelegramBotConfig.OptionKey));

            services.AddSingleton<TelegramBotInitializer>();
            services.AddSingleton<TelegramLogger>();

            return services;
        }
    }
}
