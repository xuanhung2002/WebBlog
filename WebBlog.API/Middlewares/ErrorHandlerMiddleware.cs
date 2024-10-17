using System.Text.Json;
using WebBlog.Application.Exceptions;

namespace WebBlog.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case UnauthorizeException:
                        response.StatusCode = StatusCodes.Status401Unauthorized;
                        break;
                }
            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
            }
            
        }

    }
}
