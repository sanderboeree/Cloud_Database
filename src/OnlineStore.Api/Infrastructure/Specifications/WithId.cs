using OnlineStore.Api.Domain;
using System;
using System.Linq.Expressions;

namespace OnlineStore.Api.Infrastructure.Specifications
{
    public class WithId<TEntity> : SpecificationBase<TEntity> where TEntity : Entity
    {
        public Guid Id { get; }

        public WithId(Guid id)
        {
            Id = id;
        }

        public override Expression<Func<TEntity, bool>> Criteria => entity => entity.Id == Id;
    }
}
