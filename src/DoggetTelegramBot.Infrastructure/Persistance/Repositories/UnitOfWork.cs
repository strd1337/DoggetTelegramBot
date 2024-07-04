using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.TransactionEntity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Infrastructure.Persistance.Processors;
using System.Transactions;
using Transaction = DoggetTelegramBot.Domain.Models.TransactionEntity.Transaction;
using ErrorOr;

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
                where TEntity : Entity
                where TId : class
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

        public async Task<ErrorOr<bool>> ProcessTransactionAsync(
            Transaction transaction,
            CancellationToken cancellationToken)
        {
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);

            var inventoryRepository = GetRepository<Inventory, InventoryId>();
            var transactionRepository = GetRepository<Transaction, TransactionId>();
            var userRepository = GetRepository<User, UserId>();

            TransactionProcessor processor = new(
                inventoryRepository,
                transactionRepository,
                userRepository);

            var result = await processor.ProcessTransactionAsync(transaction, cancellationToken);

            if (!result.IsError)
            {
                await SaveChangesAsync(cancellationToken);
                scope.Complete();
            }

            return result;
        }

        public int SaveChanges() => dbContext.SaveChanges();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            await dbContext.SaveChangesAsync(cancellationToken);

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
