using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IGenericRepository<TEntity, TId> GetRepository<TEntity, TId>(
            bool hasCustomRepository = false)
                where TEntity : Entity<TId>
                where TId : ValueObject;
    }
}
