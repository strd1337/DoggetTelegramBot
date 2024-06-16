using DoggetTelegramBot.Application.Common.Behaviors;
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

                config.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
            });

            return services;
        }
    }
}
