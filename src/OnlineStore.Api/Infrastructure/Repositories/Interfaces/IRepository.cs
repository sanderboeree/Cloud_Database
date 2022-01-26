using OnlineStore.Api.Domain;
using OnlineStore.Api.Infrastructure.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> FindOneAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<TEntity>> FindAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

        Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task RawSqlAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);
    }
}
