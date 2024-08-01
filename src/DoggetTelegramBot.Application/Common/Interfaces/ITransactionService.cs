using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Common.Interfaces
{
    public interface ITransactionService
    {
        Task<ErrorOr<bool>> ExecuteServiceFeeAsync(
            List<UserId> fromUserIds,
            decimal amount,
            CancellationToken cancellationToken = default);

        Task<ErrorOr<bool>> ExecuteTransferFundsAsync(
            UserId fromUserId,
            UserId toUserId,
            decimal amount,
            CancellationToken cancellationToken = default);

        Task<ErrorOr<bool>> ExecutePurchaseItemsAsync(
            UserId fromUserId,
            decimal amount,
            CancellationToken cancellationToken = default);

        Task<ErrorOr<bool>> ExecuteRewardUserAsync(
            UserId toUserId,
            decimal amount,
            CancellationToken cancellationToken = default);

        Task<ErrorOr<bool>> ExecuteUserPenaltyAsync(
            UserId fromUserId,
            decimal amount,
            CancellationToken cancellationToken = default);
    }
}
