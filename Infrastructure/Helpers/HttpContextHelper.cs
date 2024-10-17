using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using WebBlog.Infrastructure.Identity;

namespace WebBlog.Infrastructure.Helpers
{
    public static class HttpContextHelper
    {
        public static IConfiguration Configuration { get; set; }
        public static string GenerateToken(AppUser user, IConfiguration configuration)
        {
            var tokenKey = configuration["Tokens:Key"];
            var issuer = configuration["Tokens:Issuer"];
            var audience = configuration["Tokens:Audience"];
            var expire = configuration["Tokens:Expire"];
            var roles = user.Roles;
            var dateExpire = DateTime.UtcNow.AddHours(7).AddMinutes(int.Parse(expire));
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(roles)),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer, audience, claims, expires: dateExpire, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static Guid GetCurrentUserId(this HttpContext context)
        {
            var currentUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(currentUserId, out var userId);
            return userId;
        }

        public static string? GetCurrentUserName(this HttpContext context)
        {
            var currentUserName = context.User.FindFirstValue(ClaimTypes.Name);
            return currentUserName;
        }
        public static string? GetCurrentUserEmail(this HttpContext context)
        {
            var currentUserEmail = context.User.FindFirstValue(ClaimTypes.Email);
            return currentUserEmail;
        }
    }
}
