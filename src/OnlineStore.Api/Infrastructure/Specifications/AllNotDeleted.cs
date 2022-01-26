using OnlineStore.Api.Domain;
using System;
using System.Linq.Expressions;

namespace OnlineStore.Api.Infrastructure.Specifications
{
    public class AllNotDeleted<TEntity> : SpecificationBase<TEntity> where TEntity : SoftDeleteEntity
    {
        public override Expression<Func<TEntity, bool>> Criteria => entity => !entity.IsDeleted;
    }
}
