using System;
using OnlineStore.Api.Application.Users;
using OnlineStore.Api.Domain.Orders;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Api.Application.Orders
{
    public class OrderDto : Dto<Order>
    {
        public OrderStatus Status { get; set; }

        public DateTime? ShippingDate { get; set; }

        public PaymentType PaymentType { get; set; }

        public Guid UserId { get; set; }
        public virtual UserDto User { get; set; }

        public Guid? BillingAddressId { get; set; }
        public virtual AddressDto BillingAddress { get; set; }

        public Guid? ShippingAddressId { get; set; }
        public virtual AddressDto ShippingAddress { get; set; }

        public ICollection<OrderProductDto> OrderProducts { get; set; } = new List<OrderProductDto>();

        public OrderDto()
        {
        }

        public OrderDto(Order entity)
        {
            Id = entity.Id;
            Status = entity.Status;
            ShippingDate = entity.ShippingDate;
            PaymentType = entity.PaymentType;
            UserId = entity.UserId;
            BillingAddressId = entity.BillingAddressId;
            ShippingAddressId = entity.ShippingAddressId;

            if (entity.User != null)
            {
                User = new UserDto(entity.User);
            }

            if (entity.BillingAddress != null)
            {
                BillingAddress = new AddressDto(entity.BillingAddress);
            }

            if (entity.ShippingAddress != null)
            {
                ShippingAddress = new AddressDto(entity.ShippingAddress);
            }

            if (entity.OrderProducts.Any())
            {
                OrderProducts = entity.OrderProducts.Select(op => new OrderProductDto(op)).ToList();
            }
        }

        public override Order ToEntity(Order entity = null)
        {
            entity ??= new Order();

            entity.Id = Id;
            entity.Status = Status;
            entity.ShippingDate = ShippingDate;
            entity.PaymentType = PaymentType;
            entity.UserId = UserId;
            entity.BillingAddressId = BillingAddressId;
            entity.ShippingAddressId = ShippingAddressId;

            return entity;
        }
    }
}