using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Application.Registrations;
using OnlineStore.Api.Application.Users.Interfaces;
using OnlineStore.Api.Infrastructure.ExceptionHandlers;
using System.Collections.Generic;
using OnlineStore.Api.Application.Users;
using OnlineStore.Api.Infrastructure.EntityFramework.Data;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.Crud.Interfaces;
using System.Linq;

namespace OnlineStore.Api.Controllers
{
    public class RegistrationsController : BaseController
    {
        private readonly IUserCrudService _userCrudService;
        private readonly ICrudService<Address, AddressDto> _addressCrudService;

        private readonly IValidator<RegistrationDto> _registrationValidator;

        public RegistrationsController(IUserCrudService userCrudService,
            ICrudService<Address, AddressDto> addressCrudService,

            IValidator<RegistrationDto> registrationValidator
            )
        {
            _userCrudService = userCrudService;
            _addressCrudService = addressCrudService;

            _registrationValidator = registrationValidator;
        }

        /// <summary>
        /// Registers a new customer
        /// </summary>
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 409)]
        [HttpPost()]
        public async Task<IActionResult> CreateAsync([FromBody] RegistrationDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            await _registrationValidator.ValidateAndThrowAsync(dto, cancellationToken: cancellationToken);
            var exists = await _userCrudService.ExistsAsync(dto.EmailAddress);
            if (exists)
            {
                return Forbid();
            }

            var user = new UserDto
            {
                Name = $"{dto.FirstName} {dto.LastName}",
                Email = dto.EmailAddress,
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password,
            };

            var roles = new List<string>() { RoleData.Customer };
            user = await _userCrudService.CreateAsync(user, roles);

            if (dto.Addresses.Any())
            {
                foreach (var address in dto.Addresses)
                {
                    address.UserId = user.Id;
                    await _addressCrudService.CreateAsync(address, cancellationToken);
                }
            }

            return NoContent();
        }
    }
}
