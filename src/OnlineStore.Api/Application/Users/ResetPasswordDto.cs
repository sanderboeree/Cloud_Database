using System;

namespace OnlineStore.Api.Application.Users
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
