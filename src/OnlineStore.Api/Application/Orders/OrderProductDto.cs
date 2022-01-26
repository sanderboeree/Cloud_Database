using OnlineStore.Api.Domain.Orders;
using System;

namespace OnlineStore.Api.Application.Orders
{
    public class OrderProductDto : Dto<OrderProduct>
    {
        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
        public virtual OrderDto Order { get; set; }

        public Guid ProductId { get; set; }
        public virtual ProductDto Product { get; set; }

        public OrderProductDto()
        {
        }

        public OrderProductDto(OrderProduct entity)
        {
            Id = entity.Id;
            Quantity = entity.Quantity;
            OrderId = entity.OrderId;
            ProductId = entity.ProductId;

            if (entity.Product != null)
            {
                Product = new ProductDto(entity.Product);
            }
        }

        public override OrderProduct ToEntity(OrderProduct entity = null)
        {
            entity ??= new OrderProduct();

            entity.Id = Id;
            entity.OrderId = OrderId;
            entity.ProductId = ProductId;
            entity.Quantity = Quantity;

            return entity;
        }
    }
}



