using OnlineStore.Api.Domain;
using System;

namespace OnlineStore.Api.Infrastructure.Events
{
    public class EntityEvent<TEntity> : Event where TEntity : Entity
    {
        public EntityEventType Type { get; }
        public TEntity Entity { get; }
        public Guid? UserId { get; set; }

        public EntityEvent(EntityEventType type, TEntity entity, Guid? userId = null)
        {
            Type = type;
            Entity = entity;
            UserId = userId;
        }
    }
}
