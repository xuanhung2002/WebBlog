using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;

namespace WebBlog.API.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AccessAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly IList<string> _roles = new List<string>();
        public AccessAttribute(params string[] roles)
        {
            _roles = roles;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var user = context.HttpContext.User;
            if(user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" })
                { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
            // Check if the user has any of the required roles
            var userRolesClaims = user.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userRoles = JsonSerializer.Deserialize<List<string>>(userRolesClaims);

            var hasAccess = _roles.Any(role => userRoles.Contains(role, StringComparer.OrdinalIgnoreCase));

            if (!hasAccess)
            {
                // User is authenticated but does not have the required role
                context.Result = new JsonResult(new { message = "Forbidden: Access is denied" })
                { StatusCode = StatusCodes.Status403Forbidden };
            }

            // Since this is a Task method, use Task.CompletedTask to indicate it's done
            await Task.CompletedTask;
        }
    }
}
