using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                Api.Infrastructure.Exceptions.NotFoundException => StatusCodes.Status404NotFound,
                Api.Infrastructure.Exceptions.UnauthorizedException => StatusCodes.Status401Unauthorized,
                Api.Infrastructure.Exceptions.BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                success = false,
                message = exception.Message,
                statusCode = context.Response.StatusCode,
                // Solo en desarrollo
#if DEBUG
                stackTrace = exception.StackTrace
#endif
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
