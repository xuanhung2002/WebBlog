using System.Security.Claims;
using WebBlog.Infrastructure.Helpers;

namespace WebBlog.API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var validationResult = HttpContextHelper.ValidateToken(token, configuration);
            if(validationResult != null)
            {
                var claimsIdentity = new ClaimsIdentity(validationResult.Claims, "Bearer");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                context.User = claimsPrincipal;
            }

            await _next(context);
        }
    }
}
