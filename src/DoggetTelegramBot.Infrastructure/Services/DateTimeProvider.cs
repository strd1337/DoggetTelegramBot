using DoggetTelegramBot.Application.Common.Services;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.Now;
    }
}
