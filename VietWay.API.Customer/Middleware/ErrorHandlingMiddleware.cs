using IdGen;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text.Json;

namespace VietWay.API.Customer.Middleware
{
    public class ErrorHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

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
        public static Task HandleServerExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = JsonSerializer.Serialize(new
            {
                error = ex.Message
            });
            context.Response.ContentType = "application/json";
            var header = new KeyValuePair<string, StringValues>("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add(header);
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
