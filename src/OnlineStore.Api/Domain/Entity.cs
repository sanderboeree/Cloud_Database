using System;

namespace OnlineStore.Api.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        public DateTime Modified { get; protected set; }

        public DateTime Created { get; protected set; }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}
