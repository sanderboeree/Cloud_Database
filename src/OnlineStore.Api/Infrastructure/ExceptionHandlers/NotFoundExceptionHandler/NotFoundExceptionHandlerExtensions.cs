using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers.NotFoundExceptionHandler
{
    public static class NotFoundExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseOnlineStoreNotFoundExceptionHandler(this IApplicationBuilder app) => app.UseOnlineStoreNotFoundExceptionHandler(_ => { });

        public static IApplicationBuilder UseOnlineStoreNotFoundExceptionHandler(this IApplicationBuilder app, Action<ExceptionHandlerOptions> setupAction)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = new ExceptionHandlerOptions();

            setupAction(options);

            return app.UseMiddleware<NotFoundExceptionHandlerMiddleware>(Options.Create(options));
        }
    }
}
