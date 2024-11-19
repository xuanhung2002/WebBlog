using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebBlog.Application.ExternalServices;
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

        public async Task Invoke(HttpContext context, IAuthService authService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var validationResult = (JwtSecurityToken)authService.ValidateToken(token);
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
