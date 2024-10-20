using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace VietWay.Middleware
{
    public class ErrorHandlingMiddleware(RequestDelegate next)
    {
        public readonly RequestDelegate _next = next;

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
                error = ex.Message.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries),
                inner = ex.InnerException?.Message.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries),
                detail = ex.StackTrace?.Split(["\r\n","\n"], StringSplitOptions.RemoveEmptyEntries)
            });
            context.Response.ContentType = "application/json";
            var header = new KeyValuePair<string, StringValues>("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add(header);
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
