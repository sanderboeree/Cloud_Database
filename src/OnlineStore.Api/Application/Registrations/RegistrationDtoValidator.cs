﻿using FluentValidation;
using OnlineStore.Api.Infrastructure.ExceptionHandlers;
using OnlineStore.Api.Infrastructure.FluentValidation;

namespace OnlineStore.Api.Application.Registrations
{
    public class RegistrationDtoValidator : OnlineStoreAbstractValidator<RegistrationDto>
    {
        public RegistrationDtoValidator()
        {
            RuleFor(registration => registration.FirstName)
                .NotEmpty()
                .WithErrorCode(ErrorCode.HttpStatus400.RequiredValue)
                .WithMessage(ErrorCode.HttpStatus400.RequiredValueMessage);

            RuleFor(registration => registration.LastName)
                .NotEmpty()
                .WithErrorCode(ErrorCode.HttpStatus400.RequiredValue)
                .WithMessage(ErrorCode.HttpStatus400.RequiredValueMessage);

            RuleFor(registration => registration.EmailAddress)
                .NotEmpty()
                .WithErrorCode(ErrorCode.HttpStatus400.RequiredValue)
                .WithMessage(ErrorCode.HttpStatus400.RequiredValueMessage)
                .MaximumLength(250)
                .WithErrorCode(ErrorCode.HttpStatus400.InvalidLength)
                .WithMessage(ErrorCode.HttpStatus400.InvalidLengthMessage)
                .EmailAddress()
                .WithErrorCode(ErrorCode.HttpStatus400.InvalidEmailAddress)
                .WithMessage(ErrorCode.HttpStatus400.InvalidEmailAddressMessage);

            RuleFor(registration => registration.Password)
                .NotEmpty()
                .WithErrorCode(ErrorCode.HttpStatus400.RequiredValue)
                .WithMessage(ErrorCode.HttpStatus400.RequiredValueMessage)
                .Matches(@"\d")
                .WithErrorCode(ErrorCode.HttpStatus400.InvalidValueMessage)
                .WithMessage("Must contain at least 1 digit")
                .Matches(@"[A-Z]")
                .WithErrorCode(ErrorCode.HttpStatus400.InvalidValueMessage)
                .WithMessage("Must contain at least 1 capital letter")
                .Matches(@"[a-z]")
                .WithErrorCode(ErrorCode.HttpStatus400.InvalidValueMessage)
                .WithMessage("Must contain at least 1 letter")
                .Matches(@"[@!\$\.%\*\?\&#]")
                .WithErrorCode(ErrorCode.HttpStatus400.InvalidValueMessage)
                .WithMessage("Must contain at least 1 special character")
                .MinimumLength(6)
                .WithErrorCode(ErrorCode.HttpStatus400.InvalidLength)
                .WithMessage(ErrorCode.HttpStatus400.InvalidLengthMessage);
        }
    }
}
