using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers.GlobalExceptionHandler
{
    public static class GlobalExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseOnlineStoreGlobalExceptionHandler(this IApplicationBuilder app) => app.UseOnlineStoreGlobalExceptionHandler(_ => { });

        public static IApplicationBuilder UseOnlineStoreGlobalExceptionHandler(this IApplicationBuilder app, Action<ExceptionHandlerOptions> setupAction)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = new ExceptionHandlerOptions();

            setupAction(options);

            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>(Options.Create(options));
        }
    }
}
