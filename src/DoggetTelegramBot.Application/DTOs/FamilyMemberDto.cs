using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;

namespace DoggetTelegramBot.Application.DTOs
{
    public record FamilyMemberDto(
        string? Username,
        FamilyRole Role);
}
