using System;

namespace OnlineStore.Api.Application.Users
{
    public class ActivateAccountDto
    {
        public string Token { get; set; }

        public Guid UserId { get; set; }
    }
}
