using OnlineStore.Api.Application;
using OnlineStore.Api.Domain;
using OnlineStore.Api.Infrastructure.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.Crud.Interfaces
{
    public interface ICrudService<TEntity, TDto>
        where TEntity : Entity
        where TDto : Dto<TEntity>
    {
        Task<IReadOnlyCollection<TDto>> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<TDtoAlternate>> GetAsync<TDtoAlternate>(ISpecification<TEntity> spec, CancellationToken cancellationToken = default) where TDtoAlternate : Dto<TEntity>;
        Task<TDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TDtoAlternate> GetByIdAsync<TDtoAlternate>(Guid id, CancellationToken cancellationToken = default) where TDtoAlternate : Dto<TEntity>;
        Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default);
        Task<TDtoAlternate> CreateAsync<TDtoAlternate>(TDtoAlternate dto, CancellationToken cancellationToken = default) where TDtoAlternate : Dto<TEntity>;
        Task<TDto> UpdateAsync(Guid id, TDto dto, CancellationToken cancellationToken = default, bool ignoreAccessCheck = false);
        Task<TDtoAlternate> UpdateAsync<TDtoAlternate>(Guid id, TDtoAlternate dto, CancellationToken cancellationToken = default, bool ignoreAccessCheck = false) where TDtoAlternate : Dto<TEntity>;
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
        Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
        Task CheckReadAccessAsync(Guid id, CancellationToken cancellationToken);
        Task CheckWriteAccessAsync(Guid id, CancellationToken cancellationToken);
    }
}
