using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace OnlineStore.Api.Domain.Orders
{
    public class Role : IdentityRole<Guid>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}