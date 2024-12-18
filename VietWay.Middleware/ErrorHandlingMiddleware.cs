using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using VietWay.Util.CustomExceptions;

namespace VietWay.Middleware
{
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleServerExceptionAsync(context, ex);
            }
        }
        public Task HandleServerExceptionAsync(HttpContext context, Exception ex)
        {
            var code = ex switch
            {
                InvalidActionException => HttpStatusCode.BadRequest,
                InvalidInfoException => HttpStatusCode.BadRequest,
                ResourceAlreadyExistsException => HttpStatusCode.BadRequest,
                ResourceNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };
            if (code == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(ex, "An unhandled exception has occurred");
            }
            string? data = null;
            var result = JsonSerializer.Serialize(new
            {
                message = code != HttpStatusCode.InternalServerError ? ex.Message : "INTERNAL_SERVER_ERROR",
                statusCode = code,
                data
            });
            context.Response.ContentType = "application/json";
            var header = new KeyValuePair<string, StringValues>("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add(header);
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}