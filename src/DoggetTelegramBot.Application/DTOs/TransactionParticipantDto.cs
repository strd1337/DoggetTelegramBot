using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.DTOs
{
    public record TransactionParticipantDto(UserId UserId, InventoryId InventoryId);
}
