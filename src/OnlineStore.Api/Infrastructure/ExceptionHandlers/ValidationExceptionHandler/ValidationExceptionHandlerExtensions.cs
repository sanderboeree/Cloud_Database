using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers.ValidationExceptionHandler
{
    public static class ValidationExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseOnlineStoreValidationExceptionHandler(this IApplicationBuilder app) => app.UseOnlineStoreValidationExceptionHandler(_ => { });

        public static IApplicationBuilder UseOnlineStoreValidationExceptionHandler(this IApplicationBuilder app, Action<ExceptionHandlerOptions> setupAction)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = new ExceptionHandlerOptions();

            setupAction(options);

            return app.UseMiddleware<ValidationExceptionHandlerMiddleware>(Options.Create(options));
        }
    }
}
