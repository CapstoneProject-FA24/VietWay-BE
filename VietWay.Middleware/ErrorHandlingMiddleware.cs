using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using VietWay.Util.CustomExceptions;

namespace VietWay.Middleware
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
                await HandleExceptionAsync(context, ex);
            }
        }
        public static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            HttpStatusCode code;
            ResponseModel<string> response;

            if (ex is InvalidInfoException)
            {
                code = HttpStatusCode.BadRequest;
                response = new ResponseModel<string> { StatusCode = (int)code, Message = ex.Message };
            }
            else if (ex is ResourceNotFoundException)
            {
                code = HttpStatusCode.NotFound;
                response = new ResponseModel<string> { StatusCode = (int)code, Message = ex.Message };
            }
            else if (ex is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
                response = new ResponseModel<string> { StatusCode = (int)code, Message = ex.Message };
            }
            else
            {
                code = HttpStatusCode.InternalServerError;
                response = new ResponseModel<string> { StatusCode = (int)code, Message = "Internal Server Error" };
            }

            var result = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            var header = new KeyValuePair<string, StringValues>("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add(header);
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
