using System.Linq.Expressions;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoggetTelegramBot.Infrastructure.Persistance.Repositories
{
    public class GenericRepository<TEntity, TId>(
        BotDbContext dbContext) : IGenericRepository<TEntity, TId>
            where TEntity : Entity<TId>
            where TId : ValueObject
    {
        public async Task<TEntity?> GetByIdAsync(
            TId id,
            CancellationToken cancellationToken = default) => await dbContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id == id,
                    cancellationToken);

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default) => await dbContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(predicate, cancellationToken);

        public async Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default) => await dbContext
                .Set<TEntity>()
                .AddAsync(entity, cancellationToken);

        public Task UpdateAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(List<TEntity> entities)
        {
            List<Task> updateTasks = [];

            foreach (var entity in entities)
            {
                updateTasks.Add(UpdateAsync(entity));
            }

            return Task.WhenAll(updateTasks);
        }

        public Task RemoveAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public IQueryable<TEntity> GetAll() => dbContext.Set<TEntity>();

        public async Task<ICollection<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default) => await dbContext
                .Set<TEntity>()
                .ToListAsync(cancellationToken);

        public IQueryable<TEntity> GetAll(string include) => dbContext
                .Set<TEntity>()
                .Include(include);

        public IQueryable<TEntity> GetWhere(
            Expression<Func<TEntity, bool>> predicate) => dbContext
                .Set<TEntity>()
                .Where(predicate);

        public IQueryable<TEntity> GetWhere(
            Expression<Func<TEntity, bool>> predicate,
            string include) => GetWhere(predicate).Include(include);

        public IQueryable<TEntity> GetAll(string include, string include2) => dbContext
                .Set<TEntity>()
                .Include(include)
                .Include(include2);

        public async Task<int> CountAllAsync(
            CancellationToken cancellationToken = default) => await dbContext
                .Set<TEntity>()
                .CountAsync(cancellationToken);

        public async Task<int> CountWhereAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default) => await dbContext
                .Set<TEntity>()
                .CountAsync(predicate, cancellationToken);

        public async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default) => await dbContext
                .Set<TEntity>()
                .AnyAsync(predicate, cancellationToken);

        public async Task<ICollection<TEntity>> GetWhereAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken) => await dbContext
                .Set<TEntity>()
                .Where(predicate)
                .ToArrayAsync(cancellationToken);

        public IQueryable<TEntity> GetAll(params string[] includes)
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            foreach (string include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default,
            params string[] includes)
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            foreach (string include in includes)
            {
                query = query.Include(include);
            }

            return await query
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
