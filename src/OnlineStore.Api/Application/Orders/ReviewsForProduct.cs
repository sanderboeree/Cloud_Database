using OnlineStore.Api.Infrastructure.Specifications;
using System;
using OnlineStore.Api.Domain.Orders;
using System.Linq.Expressions;

namespace OnlineStore.Api.Application.Orders
{
    public class ReviewsForProduct : SpecificationBase<Review>
    {
        private readonly Guid _productId;

        public override Expression<Func<Review, bool>> Criteria => entity => entity.ProductId == _productId && !entity.IsDeleted;

        public ReviewsForProduct(Guid productId)
        {
            _productId = productId;
        }
    }
}