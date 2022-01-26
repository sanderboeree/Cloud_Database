using OnlineStore.Api.Domain;
using OnlineStore.Api.Infrastructure.Extensions;
using OnlineStore.Api.Infrastructure.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OnlineStore.Api.Infrastructure.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : Entity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query
                    .Where(specification.Criteria);
            }

            query = specification.Includes
                .Aggregate(query, (current, include) => current.Include(include));

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy, specification.SortDirection);
            }

            if (specification.GroupBy != null)
            {
                query = query
                    .GroupBy(specification.GroupBy).SelectMany(group => group);
            }

            return query;
        }
    }
}
