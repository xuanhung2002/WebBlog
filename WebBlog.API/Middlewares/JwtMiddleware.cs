using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebBlog.Application;
using WebBlog.Application.Common;
using WebBlog.Application.ExternalServices;

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
                var claimValue = context.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;

                if (!Guid.TryParse(claimValue, out var userId))
                {
                    userId = Guid.Empty;
                }
                RuntimeContext.CurrentUser = new CUserBase
                {
                    Id = userId,
                };
            }

            await _next(context);
        }
    }
}
