using OnlineStore.Api.Infrastructure.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OnlineStore.Api.Infrastructure.Specifications
{
    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        private Func<T, bool> _compiledCriteria;
        private Func<T, bool> CompiledCriteria => _compiledCriteria ??= Criteria.Compile();

        public abstract Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public string OrderBy { get; private set; }
        public SortDirection SortDirection { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }
        public int? Take { get; private set; }
        public int? Skip { get; private set; }

        public bool IsSatisfiedBy(T obj)
        {
            return CompiledCriteria(obj);
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        protected void ApplyOrderBy(string orderBy, SortDirection sortDirection = SortDirection.Ascending)
        {
            OrderBy = orderBy;
            SortDirection = sortDirection;
        }


        protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }
    }
}
