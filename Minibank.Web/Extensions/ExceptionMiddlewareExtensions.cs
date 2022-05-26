using Microsoft.AspNetCore.Builder;
using Minibank.Web.Middleware;

namespace Minibank.Web.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<ValidationExceptionMiddleware>();
            app.UseMiddleware<ObjectNotFoundExceptionMiddleware>();
        }
    }
}
