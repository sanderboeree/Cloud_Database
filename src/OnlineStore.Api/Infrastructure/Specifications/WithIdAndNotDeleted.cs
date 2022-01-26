using OnlineStore.Api.Domain;
using System;
using System.Linq.Expressions;

namespace OnlineStore.Api.Infrastructure.Specifications
{
    public class WithIdAndNotDeleted<TEntity> : WithId<TEntity> where TEntity : SoftDeleteEntity
    {
        public WithIdAndNotDeleted(Guid id) : base(id)
        {
        }

        public override Expression<Func<TEntity, bool>> Criteria => entity => entity.Id == Id && !entity.IsDeleted;
    }
}
