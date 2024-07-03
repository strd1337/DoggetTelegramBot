using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.TransactionEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IGenericRepository<TEntity, TId> GetRepository<TEntity, TId>(
            bool hasCustomRepository = false)
                where TEntity : Entity
                where TId : class;

        Task<ErrorOr<bool>> ProcessTransactionAsync(
            Transaction transaction,
            CancellationToken cancellationToken);
    }
}
