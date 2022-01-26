using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers.UpdateExceptionHandler
{
    public static class UpdateExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseOnlineStoreUpdateExceptionHandler(this IApplicationBuilder app) => app.UseOnlineStoreUpdateExceptionHandler(_ => { });

        public static IApplicationBuilder UseOnlineStoreUpdateExceptionHandler(this IApplicationBuilder app, Action<ExceptionHandlerOptions> setupAction)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = new ExceptionHandlerOptions();

            setupAction(options);

            return app.UseMiddleware<UpdateExceptionHandlerMiddleware>(Options.Create(options));
        }
    }
}
