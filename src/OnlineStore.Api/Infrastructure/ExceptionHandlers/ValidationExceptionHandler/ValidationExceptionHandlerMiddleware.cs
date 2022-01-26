using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers.ValidationExceptionHandler
{
    public class ValidationExceptionHandlerMiddleware : ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlerOptions _options;
        private readonly ILogger _logger;

        public ValidationExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory,
            IOptions<ExceptionHandlerOptions> options)
        {
            _next = next;
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<ValidationExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                try
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 400;
                    context.Response.OnStarting(ClearCacheHeadersDelegate, context.Response);
                    context.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(CreateErrorResponse(ex.Errors), _options.SerializerSettings), Encoding.UTF8);

                    return;
                }
                catch (Exception ex2)
                {
                    _logger.LogError(ex2, "An exception was thrown attempting to execute the error handler.");
                }

                throw;
            }
        }

        private Error CreateErrorResponse(IEnumerable<ValidationFailure> errors) => new ApiError
        {
            RefId = CreateRefId(),
            ErrorCode = ErrorCode.HttpStatus400.Default,
            ErrorMessage = ErrorCode.HttpStatus400.DefaultMessage,
            ErrorDetails = errors.GroupBy(error => error.PropertyName).Select(e => new ErrorDetails
            {
                PropertyName = e.Key,
                Errors = e.Select(vf => new Error
                {
                    ErrorCode = vf.ErrorCode,
                    ErrorMessage = vf.ErrorMessage
                }).ToList()
            }).ToList()
        };
    }
}
