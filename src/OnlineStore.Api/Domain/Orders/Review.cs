using System;

namespace OnlineStore.Api.Domain.Orders
{
    public class Review : SoftDeleteEntity
    {
        public string Feedback { get; set; }

        public double Rating { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
