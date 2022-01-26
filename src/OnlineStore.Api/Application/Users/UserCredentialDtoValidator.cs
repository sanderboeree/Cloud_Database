using FluentValidation;
using OnlineStore.Api.Infrastructure.ExceptionHandlers;
using OnlineStore.Api.Infrastructure.FluentValidation;

namespace OnlineStore.Api.Application.Users
{
    public class UserCredentialDtoValidator : OnlineStoreAbstractValidator<UserCredentialDto>
    {
        public UserCredentialDtoValidator()
        {
            RuleFor(dto => dto.Email)
                .NotEmpty()
                .WithErrorCode(ErrorCode.HttpStatus400.RequiredValue)
                .WithMessage(ErrorCode.HttpStatus400.RequiredValueMessage)
                .EmailAddress()
                .WithErrorCode(ErrorCode.HttpStatus400.InvalidEmailAddress)
                .WithMessage(ErrorCode.HttpStatus400.InvalidEmailAddressMessage);

            RuleFor(dto => dto.Password)
                .NotEmpty()
                .WithErrorCode(ErrorCode.HttpStatus400.RequiredValue)
                .WithMessage(ErrorCode.HttpStatus400.RequiredValueMessage);
        }
    }
}
