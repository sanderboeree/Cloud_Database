using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers
{
    public abstract class ExceptionHandlerMiddleware
    {
        protected readonly Func<object, Task> ClearCacheHeadersDelegate;

        protected ExceptionHandlerMiddleware()
        {
            ClearCacheHeadersDelegate = ClearCacheHeaders;
        }

        protected string CreateRefId()
        {
            return Guid.NewGuid().ToString("N");
        }

        protected Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }
    }
}
