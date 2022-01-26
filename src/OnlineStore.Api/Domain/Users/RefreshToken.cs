using System;

namespace OnlineStore.Api.Domain.Orders
{
    public class RefreshToken : Entity
    {
        public string JwtId { get; set; }
        public DateTime Expires { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
