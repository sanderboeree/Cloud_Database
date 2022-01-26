using System;
using System.Collections.Generic;

namespace OnlineStore.Api.Domain.Orders
{
    public class Order : SoftDeleteEntity
    {
        public OrderStatus Status { get; set; }

        public DateTime? ShippingDate { get; set; }

        public PaymentType PaymentType { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid? BillingAddressId { get; set; }
        public virtual Address BillingAddress { get; set; }

        public Guid? ShippingAddressId { get; set; }
        public virtual Address ShippingAddress { get; set; }

        public ISet<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
    }
}