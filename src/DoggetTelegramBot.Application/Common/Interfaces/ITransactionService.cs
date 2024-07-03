using DoggetTelegramBot.Domain.Models.ItemEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Common.Interfaces
{
    public interface ITransactionService
    {
        Task<ErrorOr<bool>> ExecuteServiceFeeAsync(
            List<UserId> userIds,
            decimal amount,
            CancellationToken cancellationToken = default);

        Task<ErrorOr<bool>> ExecuteTransferFundsAsync(
            UserId fromUserId,
            UserId toUserId,
            List<ItemId> items,
            decimal? amount = null,
            CancellationToken cancellationToken = default);

        Task<ErrorOr<bool>> ExecutePurchaseItemsAsync(
            UserId userId,
            decimal amount,
            List<ItemId> items,
            CancellationToken cancellationToken = default);

        Task<ErrorOr<bool>> ExecuteRewardUserAsync(
            UserId fromUserId,
            UserId toUserId,
            List<ItemId> items,
            decimal? amount = null,
            CancellationToken cancellationToken = default);

        Task<ErrorOr<bool>> ExecuteUserPenaltyAsync(
            UserId fromUserId,
            UserId toUserId,
            List<ItemId> items,
            decimal? amount = null,
            CancellationToken cancellationToken = default);
    }
}
