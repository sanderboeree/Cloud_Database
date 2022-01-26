using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Infrastructure.ExceptionHandlers;

namespace OnlineStore.Api.Infrastructure.FluentValidation
{
    public class OnlineStoreAbstractValidator<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            if (context.InstanceToValidate == null)
            {
                var failure = new ValidationFailure(typeof(T).Name, ErrorCode.HttpStatus400.InvalidDataTypeMessage, "Error")
                {
                    ErrorCode = ErrorCode.HttpStatus400.InvalidDataType
                };

                return new ValidationResult(new[] { failure });
            }

            return base.Validate(context);
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation = default)
        {
            if (context.InstanceToValidate == null)
            {
                var failure = new ValidationFailure(typeof(T).Name, ErrorCode.HttpStatus400.InvalidDataTypeMessage, "Error")
                {
                    ErrorCode = ErrorCode.HttpStatus400.InvalidDataType
                };

                return new ValidationResult(new[] { failure });
            }

            return await base.ValidateAsync(context, cancellation);
        }
    }
}
