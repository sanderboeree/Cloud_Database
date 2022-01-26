using System;

namespace OnlineStore.Api.Domain.Orders
{
    public class Address : Entity
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string HouseNumber { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}