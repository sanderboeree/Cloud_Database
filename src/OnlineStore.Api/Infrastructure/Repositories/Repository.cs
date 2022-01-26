using OnlineStore.Api.Domain;
using OnlineStore.Api.Infrastructure.EntityFramework;
using OnlineStore.Api.Infrastructure.Repositories.Interfaces;
using OnlineStore.Api.Infrastructure.Specifications;
using OnlineStore.Api.Infrastructure.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> FindOneAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            return await Filter(specification)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<TEntity>> FindAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            return await Filter(specification)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            return await Filter(spec)
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        public async Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!_dbContext.Entry(entity).IsKeySet)
            {
                _dbContext.Update(entity);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbEntity = await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);

            if (dbEntity == null)
            {
                throw new InvalidOperationException($"Entity with id {id} does not exist");
            }
            if (dbEntity is SoftDeleteEntity softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
            }
            else
            {
                _dbContext.Remove(dbEntity);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RawSqlAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        private IQueryable<TEntity> Filter(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>
                .GetQuery(_dbContext.Set<TEntity>().AsQueryable(), spec);
        }
    }
}
