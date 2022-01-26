using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers.NotFoundExceptionHandler
{
    public class NotFoundExceptionHandlerMiddleware : ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlerOptions _options;
        private readonly ILogger _logger;

        public NotFoundExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory,
            IOptions<ExceptionHandlerOptions> options)
        {
            _next = next;
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<NotFoundExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine(context.ToString());
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                try
                {
                    var refId = CreateRefId();
                    var error = $"Error {refId} at <{context.Request.GetDisplayUrl()}>, <Not found>";
                    _logger.LogWarning(error);

                    context.Response.Clear();
                    context.Response.StatusCode = 404;
                    context.Response.OnStarting(ClearCacheHeadersDelegate, context.Response);
                    context.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();

                    var response = new ApiError
                    {
                        RefId = refId,
                        ErrorCode = ErrorCode.HttpStatus404.Default,
                        ErrorMessage = ErrorCode.HttpStatus404.DefaultMessage
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _options.SerializerSettings), Encoding.UTF8);

                    return;
                }
                catch (Exception ex2)
                {
                    _logger.LogError(ex2, "An exception was thrown attempting to execute the error handler.");
                }

                throw;
            }
        }
    }
}
