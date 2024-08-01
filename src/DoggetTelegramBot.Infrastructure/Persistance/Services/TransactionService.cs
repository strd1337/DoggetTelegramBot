using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Domain.Models.TransactionEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Infrastructure.Persistance.Services
{
    public sealed class TransactionService(IUnitOfWork unitOfWork)
        : ITransactionService
    {
        public async Task<ErrorOr<bool>> ExecuteServiceFeeAsync(
            List<UserId> fromUserIds,
            decimal amount,
            CancellationToken cancellationToken = default)
        {
            Transaction transaction = Transaction.CreateServiceFee(fromUserIds, amount);
            return await unitOfWork.ProcessTransactionAsync(transaction, cancellationToken);
        }

        public async Task<ErrorOr<bool>> ExecuteTransferFundsAsync(
            UserId fromUserId,
            UserId toUserId,
            decimal amount,
            CancellationToken cancellationToken = default)
        {
            Transaction transaction = Transaction.CreateTransfer(fromUserId, toUserId, amount);
            return await unitOfWork.ProcessTransactionAsync(transaction, cancellationToken);
        }

        public async Task<ErrorOr<bool>> ExecutePurchaseItemsAsync(
            UserId fromUserId,
            decimal amount,
            CancellationToken cancellationToken = default)
        {
            Transaction transaction = Transaction.CreatePurchase(fromUserId, amount);
            return await unitOfWork.ProcessTransactionAsync(transaction, cancellationToken);
        }

        public async Task<ErrorOr<bool>> ExecuteRewardUserAsync(
            UserId toUserId,
            decimal amount,
            CancellationToken cancellationToken = default)
        {
            Transaction transaction = Transaction.CreateReward(toUserId, amount);
            return await unitOfWork.ProcessTransactionAsync(transaction, cancellationToken);
        }

        public async Task<ErrorOr<bool>> ExecuteUserPenaltyAsync(
            UserId fromUserId,
            decimal amount,
            CancellationToken cancellationToken = default)
        {
            Transaction transaction = Transaction.CreatePenalty(fromUserId, amount);
            return await unitOfWork.ProcessTransactionAsync(transaction, cancellationToken);
        }
    }
}
