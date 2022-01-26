using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace OnlineStore.Api.Domain.Orders
{
    public class User : IdentityUser<Guid>
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value.Length < 1)
                {
                    throw new ArgumentException("Name should not be empty");
                }
                _Name = value;
            }
        }
        public virtual ISet<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public bool IsDeleted { get; set; }
    }
}
