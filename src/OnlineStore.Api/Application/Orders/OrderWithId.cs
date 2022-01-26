using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.Specifications;
using System;

namespace OnlineStore.Api.Application.Orders
{
    public class ProductWithId : WithId<Product>
    {
        public ProductWithId(Guid id) : base(id)
        {
        }
    }
}