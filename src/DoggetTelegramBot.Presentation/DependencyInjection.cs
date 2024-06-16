using DoggetTelegramBot.Presentation.Common.ErrorHandling;

namespace DoggetTelegramBot.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(
            this IServiceCollection services)
        {
            services.AddControllers();

            services.AddGlobalExceptionHandler();

            return services;
        }

        public static IServiceCollection AddGlobalExceptionHandler(
            this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
    }
}
