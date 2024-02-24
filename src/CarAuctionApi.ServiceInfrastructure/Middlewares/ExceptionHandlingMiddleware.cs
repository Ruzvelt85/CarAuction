using System.Net;
using System.Text.Json;
using CarAuctionApi.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CarAuctionApi.ServiceInfrastructure.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await ProcessExceptionAsync(httpContext, exception);
            }
        }

        private static Task ProcessExceptionAsync(HttpContext context, Exception exception)
        {
            var payload = JsonSerializer.Serialize(new { error = exception.Message });
            context.Response.ContentType = "application/json";

            // TODO: Can be added logging
            switch (exception)
            {
                case NotFoundException:
                case ConflictException:
                case ValidationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return context.Response.WriteAsync(payload);
        }
    }
}
