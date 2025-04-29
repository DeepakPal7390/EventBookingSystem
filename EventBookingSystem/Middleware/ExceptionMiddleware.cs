using System.Net;
using System.Text.Json;

namespace EventBookingSystem.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled Exception occurred!");

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = context.Response;
            var errorResponse = new { message = ex.Message };

            switch (ex)
            {
                case EventBookingSystem.Exceptions.BadRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest; 
                    break;

                case EventBookingSystem.Exceptions.NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound; 
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError; 
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}


