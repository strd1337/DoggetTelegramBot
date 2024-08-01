using DoggetTelegramBot.Application.Common.Behaviors;
using DoggetTelegramBot.Application.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace DoggetTelegramBot.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
            });

            services.AddSingleton<CommandUsageManager>();

            return services;
        }
    }
}
