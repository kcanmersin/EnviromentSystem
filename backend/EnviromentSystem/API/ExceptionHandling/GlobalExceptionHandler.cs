﻿using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace API.Middlewares.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exceptionxxxx occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new
            {
                Title = "Server Error",
                Status = context.Response.StatusCode,
                Message = exception.Message
            });

            await context.Response.WriteAsync(result);

            return true;
        }
    }

}
