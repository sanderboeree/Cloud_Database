using System;
using System.Collections.Generic;
using System.Linq;
using OnlineStore.Api.Domain;
using OnlineStore.Api.Infrastructure.Identity.Interfaces;

namespace OnlineStore.Api.Application
{
    public abstract class Dto<TEntity> where TEntity : Entity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        protected Dto() { }

        protected Dto(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Id = entity.Id;
            Created = entity.Created;
            Modified = entity.Modified;
        }

        public abstract TEntity ToEntity(TEntity entity = null);

        public virtual void CheckWriteAccess(IUser user)
        {
            CheckReadAccess(user);
        }

        public virtual void CheckReadAccess(IUser user)
        {

        }

        protected void UpdateChildEntities<TChildEntity, TChildDto>(ICollection<TChildEntity> childEntities, ICollection<TChildDto> childDtos)
            where TChildEntity : Entity
            where TChildDto : Dto<TChildEntity>
        {
            if (childEntities == null)
            {
                throw new ArgumentNullException(nameof(childEntities));
            }

            if (childDtos == null)
            {
                throw new ArgumentNullException(nameof(childDtos));
            }

            RemoveDeletedChilds(childEntities, childDtos);
            UpdateExistingChilds(childEntities, childDtos);
            AddNewChilds(childEntities, childDtos);
        }

        private static void RemoveDeletedChilds<TChildEntity, TChildDto>(ICollection<TChildEntity> childEntities, ICollection<TChildDto> childDtos)
            where TChildEntity : Entity
            where TChildDto : Dto<TChildEntity>
        {
            childEntities
                .Where(entity => childDtos.All(dto => dto.Id != entity.Id))
                .ToList()
                .ForEach(entity => childEntities.Remove(entity));
        }

        private static void UpdateExistingChilds<TChildEntity, TChildDto>(ICollection<TChildEntity> childEntities, ICollection<TChildDto> childDtos)
            where TChildEntity : Entity
            where TChildDto : Dto<TChildEntity>
        {
            childDtos
                .Where(dto => childEntities.Any(entity => entity.Id == dto.Id))
                .ToList()
                .ForEach(dto => dto.ToEntity(childEntities.Single(entity => entity.Id == dto.Id)));
        }

        private static void AddNewChilds<TChildEntity, TChildDto>(ICollection<TChildEntity> childEntities, ICollection<TChildDto> childDtos)
            where TChildEntity : Entity
            where TChildDto : Dto<TChildEntity>
        {
            childDtos
                .Where(dto => dto.Id == Guid.Empty)
                .Select(dto => dto.ToEntity())
                .ToList()
                .ForEach(childEntities.Add);
        }
    }
}
