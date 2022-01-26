using System;

namespace OnlineStore.Api.Application.Users
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime TokenValidTo { get; set; }
        public Guid RefreshToken { get; set; }
        public DateTime RefreshTokenValidTo { get; set; }
    }
}
