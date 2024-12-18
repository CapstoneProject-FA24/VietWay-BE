using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VietWay.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Incoming request: [{requestMethod}] {requestPath}", context.Request.Method, context.Request.Path);
            _logger.LogInformation("Request data: {requestData}", JsonSerializer.Serialize(context.Request.Body));
            await _next(context);

            stopwatch.Stop();
            _logger.LogInformation("Outgoing response: {responseStatusCode} - Time taken: {elapsedMilliseconds}ms", context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}
