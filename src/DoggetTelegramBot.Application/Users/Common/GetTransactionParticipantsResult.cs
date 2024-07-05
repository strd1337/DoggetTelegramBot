using DoggetTelegramBot.Application.DTOs;

namespace DoggetTelegramBot.Application.Users.Common
{
    public record GetTransactionParticipantsResult(
        TransactionParticipantDto ToUser,
        TransactionParticipantDto? FromUser = null);
}
