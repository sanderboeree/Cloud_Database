using FluentValidation;
using OnlineStore.Api.Application.Users;
using OnlineStore.Api.Application.Users.Interfaces;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.ExceptionHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly IUserAuthenticator _userAuthenticator;

        private readonly IValidator<UserCredentialDto> _userCredentialsValidator;

        public AuthenticationController(IUserAuthenticator userAuthenticator, IValidator<UserCredentialDto> userCredentialsValidator)
        {
            _userAuthenticator = userAuthenticator;
            _userCredentialsValidator = userCredentialsValidator;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserCredentialDto userCredentialDto, CancellationToken cancellationToken)
        {
            User identityUser;

            await _userCredentialsValidator.ValidateAndThrowAsync(userCredentialDto, cancellationToken: cancellationToken);

            try
            {
                identityUser = await _userAuthenticator.AuthenticateAsync(userCredentialDto);
            }
            catch (Exception)
            {
                return Unauthorized(new ApiError { ErrorCode = ErrorCode.HttpStatus401.Default, ErrorMessage = ErrorCode.HttpStatus401.DefaultMessage });
            }

            if (identityUser == null)
            {
                return Unauthorized(new ApiError { ErrorCode = ErrorCode.HttpStatus401.Default, ErrorMessage = ErrorCode.HttpStatus401.DefaultMessage });
            }

            if (!identityUser.EmailConfirmed)
            {
                return Forbid(new ApiError { ErrorCode = ErrorCode.HttpStatus403.Default, ErrorMessage = ErrorCode.HttpStatus403.DefaultMessage });
            }

            return Ok(await _userAuthenticator.GenerateTokenAsync(identityUser, cancellationToken));
        }

        [HttpPost("Logout")]
        public IActionResult LogoutAsync()
        {
            return Ok(new TokenDto { Token = "", RefreshToken = Guid.Empty });
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] TokenDto token, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _userAuthenticator.RefreshTokenAsync(token.Token, token.RefreshToken, cancellationToken));
            }
            catch (SecurityTokenException exception)
            {
                return Unauthorized(new ApiError { ErrorCode = ErrorCode.HttpStatus401.Default, ErrorMessage = exception.Message });
            }
        }
    }
}
