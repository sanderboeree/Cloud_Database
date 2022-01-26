namespace OnlineStore.Api.Application.Users
{
    public class ValidateTokenDto
    {
        public string Token { get; set; }

        public string Subject { get; set; }

        public bool ValidateSubject { get; set; }
    }
}
