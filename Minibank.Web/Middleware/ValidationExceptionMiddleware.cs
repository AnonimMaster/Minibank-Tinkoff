using FluentValidation;
using Microsoft.AspNetCore.Http;
using Minibank.Web.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Minibank.Web.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, ILogger logger)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationException ex)
            {
                logger.Log(ex);

                var errors = ex.Errors.Select(i => $"{i.PropertyName}: {i.ErrorMessage}");

                await HandleExceptionMessageAsync(context, ex, errors).ConfigureAwait(true);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, ValidationException exception, IEnumerable<string> errors)
        {
            context.Response.ContentType = "application/json";
            const int statusCode = (int)HttpStatusCode.BadRequest;
            string message;

            if (errors.Any())
            {
                message = string.Join("", errors);
            }
            else
            {
                message = exception.Message;
            }

            var result = JsonSerializer.Serialize(new
            {
                ErrorMessage = message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
