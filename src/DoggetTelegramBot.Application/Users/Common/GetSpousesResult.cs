using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Users.Common
{
    public record class GetSpousesResult(List<User> Spouses);
}
