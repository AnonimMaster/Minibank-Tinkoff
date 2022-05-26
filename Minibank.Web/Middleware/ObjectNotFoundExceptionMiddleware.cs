using Microsoft.AspNetCore.Http;
using Minibank.Core.Exceptions;
using Minibank.Web.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Minibank.Web.Middleware
{
    public class ObjectNotFoundExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ObjectNotFoundExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, ILogger logger)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ObjectNotFoundException ex)
            {
                logger.Log(ex);

                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, ObjectNotFoundException exception)
        {
            context.Response.ContentType = "application/json";
            const int statusCode = (int)HttpStatusCode.NotFound;
            var result = JsonSerializer.Serialize(new
            {
                ErrorMessage = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
