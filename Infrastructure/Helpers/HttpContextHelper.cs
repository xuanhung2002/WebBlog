using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using WebBlog.Infrastructure.Identity;

namespace WebBlog.Infrastructure.Helpers
{
    public static class HttpContextHelper
    {
        public static string GenerateToken(AppUser user, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            var tokenKey = configuration["Tokens:Key"];
            var issuer = configuration["Tokens:Issuer"];
            var audience = configuration["Tokens:Audience"];
            var expire = configuration["Tokens:Expire"];
            var roles = userManager.GetRolesAsync(user).Result;
            var dateExpire = DateTime.UtcNow.AddHours(7).AddMinutes(int.Parse(expire));
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName + user.LastName}"),
                new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(roles)),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer, audience, claims, expires: dateExpire, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static JwtSecurityToken? ValidateToken(string? token, IConfiguration configuration)
        {
            var tokenKey = configuration["Tokens:Key"];

            if (token == null || tokenKey == null) return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
        public static RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedDate = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
            return refreshToken;
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
