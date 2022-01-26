using OnlineStore.Api.Domain;
using System;
using System.Linq.Expressions;

namespace OnlineStore.Api.Infrastructure.Specifications
{
    public class All<TEntity> : SpecificationBase<TEntity> where TEntity : Entity
    {
        public override Expression<Func<TEntity, bool>> Criteria => entity => true;
    }
}
