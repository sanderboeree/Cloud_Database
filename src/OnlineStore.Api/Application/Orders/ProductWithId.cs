using OnlineStore.Api.Infrastructure.Specifications;
using System;
using OnlineStore.Api.Domain.Orders;

public class OrderWithId : WithId<Order>
{
    public OrderWithId(Guid id) : base(id)
    {
        AddInclude(entity => entity.OrderProducts);
    }
}