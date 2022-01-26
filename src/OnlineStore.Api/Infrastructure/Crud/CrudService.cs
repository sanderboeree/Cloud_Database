using OnlineStore.Api.Application;
using OnlineStore.Api.Domain;
using OnlineStore.Api.Infrastructure.Crud.Interfaces;
using OnlineStore.Api.Infrastructure.Identity.Interfaces;
using OnlineStore.Api.Infrastructure.Repositories.Interfaces;
using OnlineStore.Api.Infrastructure.Specifications;
using OnlineStore.Api.Infrastructure.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.Crud
{
    public class CrudService<TEntity, TDto, TWithId> : ICrudService<TEntity, TDto> where TEntity : Entity
       where TDto : Dto<TEntity>
       where TWithId : WithId<TEntity>
    {
        protected readonly IRepository<TEntity> _repository;
        private readonly IUser _currentUser;

        public CrudService(IRepository<TEntity> repository, IUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<IReadOnlyCollection<TDto>> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            return await GetAsync<TDto>(spec, cancellationToken);
        }

        public async Task<IReadOnlyCollection<TDtoAlternate>> GetAsync<TDtoAlternate>(ISpecification<TEntity> spec, CancellationToken cancellationToken = default) where TDtoAlternate : Dto<TEntity>
        {
            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec));
            }

            var entities = await _repository.FindAsync(spec, cancellationToken);

            var result = entities.Select(CreateInstance<TDtoAlternate>).ToList().AsReadOnly();

            foreach (var dto in result)
            {
                dto.CheckReadAccess(_currentUser);
            }

            return result;
        }

        public async Task<TDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync<TDto>(id, cancellationToken);
        }

        public async Task<TDtoAlternate> GetByIdAsync<TDtoAlternate>(Guid id, CancellationToken cancellationToken = default) where TDtoAlternate : Dto<TEntity>
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entity = await _repository.FindOneAsync(CreateWithIdSpecification(id), cancellationToken);

            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            var dto = CreateInstance<TDtoAlternate>(entity);

            dto.CheckReadAccess(_currentUser);

            return dto;
        }

        public async Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
        {
            return await CreateAsync<TDto>(dto, cancellationToken);
        }

        public async Task<TDtoAlternate> CreateAsync<TDtoAlternate>(TDtoAlternate dto, CancellationToken cancellationToken = default) where TDtoAlternate : Dto<TEntity>
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var newEntity = dto.ToEntity();

            await _repository.SaveAsync(newEntity, cancellationToken);

            return CreateInstance<TDtoAlternate>(newEntity);
        }

        public async Task<TDto> UpdateAsync(Guid id, TDto dto, CancellationToken cancellationToken = default, bool ignoreAccessCheck = false)
        {
            return await UpdateAsync<TDto>(id, dto, cancellationToken, ignoreAccessCheck);
        }

        public async Task<TDtoAlternate> UpdateAsync<TDtoAlternate>(Guid id, TDtoAlternate dto, CancellationToken cancellationToken = default, bool ignoreAccessCheck = false) where TDtoAlternate : Dto<TEntity>
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var existingEntity = await _repository.FindOneAsync(CreateWithIdSpecification(id), cancellationToken);

            if (existingEntity == null)
            {
                throw new InvalidOperationException($"Entity with Id '{id}' not found.");
            }

            if (existingEntity.Modified != dto.Modified)
            {
                throw new DBConcurrencyException($"Entity with Id '{id}' updated elsewhere.");
            }

            var updatedEntity = dto.ToEntity(existingEntity);
            if (!ignoreAccessCheck)
            {
                dto.CheckWriteAccess(_currentUser);
            }

            await _repository.SaveAsync(updatedEntity, cancellationToken);

            return CreateInstance<TDtoAlternate>(updatedEntity);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var dto = await GetByIdAsync(id, cancellationToken);
            dto.CheckWriteAccess(_currentUser);
            await _repository.DeleteAsync(id, cancellationToken);
        }

        public async Task<bool> ExistsAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec));
            }

            return (await CountAsync(spec, cancellationToken)) > 0;
        }

        public async Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec));
            }

            return await _repository.CountAsync(spec, cancellationToken);
        }

        public async Task CheckReadAccessAsync(Guid id, CancellationToken cancellationToken)
        {
            var dto = await GetByIdAsync(id, cancellationToken);
            dto.CheckReadAccess(_currentUser);
        }

        public async Task CheckWriteAccessAsync(Guid id, CancellationToken cancellationToken)
        {
            var dto = await GetByIdAsync(id, cancellationToken);
            dto.CheckWriteAccess(_currentUser);
        }

        private static TDtoAlternate CreateInstance<TDtoAlternate>(TEntity entity) where TDtoAlternate : Dto<TEntity>
        {
            return (TDtoAlternate)Activator.CreateInstance(typeof(TDtoAlternate), entity);
        }

        private static TWithId CreateWithIdSpecification(Guid id) => (TWithId)Activator.CreateInstance(typeof(TWithId), id);
    }
}
