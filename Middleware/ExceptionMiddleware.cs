using BackEnd.Exceptions;
using System.Text.Json;

namespace BackEnd.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync (HttpContext context) {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleException(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message
                );
            }
            catch (UnauthorizedException ex)
            {
                await HandleException(
                    context,
                    StatusCodes.Status401Unauthorized,
                    ex.Message
                );
            }
            catch (NotFoundException ex)
            {
                await HandleException(
                    context,
                    StatusCodes.Status404NotFound,
                    ex.Message
                );
            }
            catch (ConflictException ex)
            {
                await HandleException(
                    context,
                    StatusCodes.Status409Conflict,
                    ex.Message
                );
            }
            catch (Exception ex)
            {
                await HandleException(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Internal server error : "+ ex.Message
                );
            }
        }


        private static async Task HandleException(
            HttpContext context,
            int statusCode,
            string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                Message = message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
