using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DoggetTelegramBot.Infrastructure.Persistance.Repositories
{
    public class UnitOfWork(
        BotDbContext dbContext,
        IDateTimeProvider dateTimeProvider) : IUnitOfWork
    {
        private readonly BotDbContext dbContext = dbContext;
        private bool isDisposed;
        private readonly Dictionary<Type, object> repositories = [];
        private readonly IDateTimeProvider dateTimeProvider = dateTimeProvider;

        public IGenericRepository<TEntity, TId> GetRepository<TEntity, TId>(
            bool hasCustomRepository = false)
                where TEntity : Entity<TId>
                where TId : ValueObject
        {
            if (hasCustomRepository)
            {
                var customRepository = dbContext
                    .GetService<IGenericRepository<TEntity, TId>>();

                if (customRepository is not null)
                {
                    return customRepository;
                }
            }

            var type = typeof(TEntity);
            if (!repositories.TryGetValue(type, out object? value))
            {
                value = new GenericRepository<TEntity, TId>(dbContext);
                repositories[type] = value;
            }

            return (IGenericRepository<TEntity, TId>)value;
        }

        public int SaveChanges() => dbContext.SaveChanges();

        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken) => await dbContext.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    repositories?.Clear();
                    dbContext.Dispose();
                }
            }
            isDisposed = true;
        }
    }
}
