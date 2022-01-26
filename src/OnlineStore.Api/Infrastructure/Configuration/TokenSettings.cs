using System;

namespace OnlineStore.Api.Infrastructure.Configuration
{
    public class TokenSettings
    {
        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public TimeSpan Expires { get; set; }
    }
}
