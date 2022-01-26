using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers.UpdateExceptionHandler
{
    public class UpdateExceptionHandlerMiddleware : ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlerOptions _options;
        private readonly ILogger _logger;

        public UpdateExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory,
            IOptions<ExceptionHandlerOptions> options)
        {
            _next = next;
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<UpdateExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is DBConcurrencyException)
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
                    context.Response.StatusCode = 409;
                    context.Response.OnStarting(ClearCacheHeadersDelegate, context.Response);
                    context.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();

                    var errorCode = ErrorCode.HttpStatus409.Default;
                    var errorMessage = ErrorCode.HttpStatus409.DefaultMessage;

                    if (ex is DbUpdateException)
                    {
                        var sqlException = ex.InnerException?.InnerException as SqlException;
                        switch (sqlException?.Number)
                        {
                            case 2627:
                            case 2601:
                                errorCode = ErrorCode.HttpStatus409.EntityExists;
                                errorMessage = ErrorCode.HttpStatus409.EntityExistsMessage;
                                break;
                        }
                    }
                    else if (ex is DBConcurrencyException)
                    {
                        errorCode = ErrorCode.HttpStatus409.EntityConcurrency;
                        errorMessage = ErrorCode.HttpStatus409.EntityConcurrencyMessage;
                    }


                    var response = new ApiError
                    {
                        RefId = refId,
                        ErrorCode = errorCode,
                        ErrorMessage = errorMessage
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
