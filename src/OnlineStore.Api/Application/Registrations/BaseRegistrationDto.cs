namespace OnlineStore.Api.Application.Registrations
{
    public class BaseRegistrationDto
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}