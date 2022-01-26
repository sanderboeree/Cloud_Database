using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OnlineStore.Api.Application.Users;
using OnlineStore.Api.Application.Users.Interfaces;
using OnlineStore.Api.Infrastructure.EntityFramework.Data;
using OnlineStore.Api.Infrastructure.ExceptionHandlers;
using OnlineStore.Api.Infrastructure.Extensions;

namespace OnlineStore.Api.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserCrudService _userCrudService;

        public UsersController(IUserCrudService userCrudService)
        {
            _userCrudService = userCrudService;
        }

        /// <summary>
        /// Gets a single user
        /// </summary>
        /// <param name="id">The id of the user</param>
        [ProducesResponseType(typeof(UserDto), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var dto = await _userCrudService.GetByIdAsync(id);
            if (!User.IsInRole(RoleData.Admin) && User.GetId() != id)
            {
                return Forbid(new ApiError { ErrorCode = ErrorCode.HttpStatus403.UserRights, ErrorMessage = ErrorCode.HttpStatus403.UserRightsMessage });
            }
            return Ok(dto);
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        [ProducesResponseType(typeof(UserDto), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            userDto.Id = id;
            var user = await _userCrudService.GetByIdAsync(userDto.Id);

            if (!User.IsInRole(RoleData.Admin))
            {
                if (!CanUpdateUser(user, userDto, User))
                {
                    return Forbid(new ApiError { ErrorCode = ErrorCode.HttpStatus403.UserRights, ErrorMessage = ErrorCode.HttpStatus403.UserRightsMessage });
                }

                if (!string.IsNullOrEmpty(userDto.Password) && !string.IsNullOrEmpty(userDto.PasswordCurrent))
                {
                    var identityResult = await _userCrudService.ChangePassword(userDto);

                    if (!identityResult.Succeeded)
                    {
                        return Forbid(new ApiError { ErrorCode = ErrorCode.HttpStatus403.InvalidCurrentPassword, ErrorMessage = ErrorCode.HttpStatus403.InvalidCurrentPasswordMessage });
                    }
                }
                userDto.Password = null;
            }

            return Ok(await _userCrudService.UpdateAsync(userDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var user = await _userCrudService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var allowedToDeletePlayerOrCoach = User.IsManager() && (user.Roles.Contains(RoleData.Customer) || user.Roles.Contains(RoleData.Customer));
            var allowedToDeleteManager = User.IsAdmin() && user.Roles.Contains(RoleData.Customer);
            if (id != User.GetId() || allowedToDeleteManager || allowedToDeletePlayerOrCoach)
            {
                return Unauthorized(new ApiError { ErrorCode = ErrorCode.HttpStatus403.UserRights, ErrorMessage = ErrorCode.HttpStatus403.UserRightsMessage });
            }
            if (user.Roles.Any(role => role == RoleData.Admin))
            {
                return Forbid(new ApiError { ErrorCode = ErrorCode.HttpStatus403.UserRights, ErrorMessage = ErrorCode.HttpStatus403.UserRightsMessage });
            }

            await _userCrudService.DeleteAsync(id);
            return Ok();
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), 200)]
        [HttpGet("exists")]
        public async Task<IActionResult> ExistsAsync([FromQuery] string email)
        {
            return Ok(await _userCrudService.ExistsAsync(email));
        }

        private static bool CanUpdateUser(UserDto previous, UserDto next, ClaimsPrincipal user)
        {
            if (user.IsInRole(RoleData.Admin) && next.Id == user.GetId())
            {
                return true;
            }
            if (previous.Roles != next.Roles)
            {
                return false;
            }

            if (!user.IsInRole(RoleData.Admin))
            {
                return false;
            }
            if (previous.Name != next.Name ||
                previous.Email != next.Email
                )
            {
                return user.GetId() == next.Id;
            }
            return true;
        }
    }
}
