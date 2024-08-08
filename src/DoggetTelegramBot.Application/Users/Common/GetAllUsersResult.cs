using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Users.Common
{
    public record GetAllUsersResult(List<User> Users);
}
