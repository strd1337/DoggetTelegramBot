using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Infrastructure.Configs;
using DoggetTelegramBot.Infrastructure.Persistance;
using DoggetTelegramBot.Infrastructure.Persistance.Repositories;
using DoggetTelegramBot.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
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

            services.AddDbContext(configuration);

            services.AddCaching();

            services.AddScoped<IScopedMediatorService, ScopedMediatorService>();

            services.AddPersistance();

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
            services.AddSingleton<ITelegramLogger, TelegramLogger>();

            return services;
        }

        public static IServiceCollection AddDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString(Constants.ConnectionString)
                ?? throw new InvalidOperationException($"{Constants.ConnectionString} connection string is not configured.");

            services.AddDbContext<BotDbContext>((options) => options.UseNpgsql(connectionString));

            return services;
        }

        public static IServiceCollection AddCaching(
            this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddSingleton<ICacheService, MemoryCacheService>();

            return services;
        }

        public static IServiceCollection AddPersistance(
           this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddCustomRepository<TEntity, TId, TRepository>(
            this IServiceCollection services)
               where TEntity : Entity<TId>
               where TId : ValueObject
               where TRepository : class, IGenericRepository<TEntity, TId>

        {
            services.AddScoped<IGenericRepository<TEntity, TId>, TRepository>();

            return services;
        }

    }
}
