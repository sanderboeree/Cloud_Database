using System;

namespace OnlineStore.Api.Domain.Orders
{
    public class OrderShipment : Entity
    {
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }

    }
}
