using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OnlineStore.Api.Application.Users
{
    public class CreateTokenDto
    {
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();

        public string UserName { get; set; }

        public string Email { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public bool GenerateRefreshToken { get; set; }

        public TimeSpan? ExpiresIn { get; set; }
    }
}