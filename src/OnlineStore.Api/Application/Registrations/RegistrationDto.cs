using OnlineStore.Api.Application.Users;
using System.Collections.Generic;

namespace OnlineStore.Api.Application.Registrations
{
    public class RegistrationDto : BaseRegistrationDto
    {
        public string PhoneNumber { get; set; }
        public IEnumerable<AddressDto> Addresses { get; set; }
    }
}
