using OnlineStore.Api.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers.GlobalExceptionHandler
{
    public class GlobalExceptionHandlerMiddleware : ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlerOptions _options;
        private readonly ILogger _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory,
            IOptions<ExceptionHandlerOptions> options)
        {
            _next = next;
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<GlobalExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OnlineStoreExeption ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                try
                {
                    var refId = CreateRefId();
                    var error = $"Error {refId} at <{context.Request.GetDisplayUrl()}>, <{ex.Message}>";
                    _logger.LogError(ex, error);

                    context.Response.Clear();
                    context.Response.StatusCode = int.Parse(ex.ErrorCode.Substring(0, 3));
                    context.Response.OnStarting(ClearCacheHeadersDelegate, context.Response);
                    context.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();

                    var response = new ApiError
                    {
                        RefId = refId,
                        ErrorCode = ex.ErrorCode,
                        ErrorMessage = ex.Message
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _options.SerializerSettings), Encoding.UTF8);

                    return;
                }
                catch (Exception ex2)
                {
                    _logger.LogError(ex2, "An exception was thrown attempting to execute the error handler.");
                }
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                try
                {
                    var refId = CreateRefId();
                    var error = $"Error {refId} at <{context.Request.GetDisplayUrl()}>, <{ex.Message}>";
                    _logger.LogError(ex, error);

                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    context.Response.OnStarting(ClearCacheHeadersDelegate, context.Response);
                    context.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();

                    var response = new ApiError
                    {
                        RefId = refId,
                        ErrorCode = ErrorCode.HttpStatus500.Default,
                        ErrorMessage = ErrorCode.HttpStatus500.DefaultMessage,
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
