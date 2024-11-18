using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebBlog.Application.Abstraction;
using WebBlog.Application.Dtos.ApiRequestDtos;
using WebBlog.Application.Dtos.AuthDtos;
using WebBlog.Application.Exceptions;
using WebBlog.Application.ExternalServices;
using WebBlog.Domain;
using WebBlog.Infrastructure.Helpers;
using WebBlog.Infrastructure.Identity;
using static WebBlog.Application.Dtos.ApiRequestDtos.AuthDtos;

namespace WebBlog.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAppDBRepository _repository;
        public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration, IAppDBRepository repository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _repository = repository;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto, string ipAddress)
        {
            var response = new AuthResponseDto();
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
                throw new UnauthorizeException("Username is not existed");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (result.Succeeded)
            {
                var refreshToken = GenerateRefreshToken(ipAddress);

                var existingTokens = new HashSet<string>(user.RefreshTokens.Select(s => s.Token));

                while (existingTokens.Contains(refreshToken.Token))
                {
                    refreshToken = GenerateRefreshToken(ipAddress);
                }

                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);

                removeOldRefreshTokens(user);

                response.AccessToken = GenerateAccessToken(user);
                response.RefreshToken = refreshToken.Token;

                return response;
            }
            else
            {
                throw new UnauthorizeException("Password is incorrect");
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token, string ipAddress)
        {
            var refreshToken = await _repository.FindAsync<RefreshToken>(s => s.Token == token);
            if(refreshToken != null)
            {
                var user = await _userManager.FindByIdAsync(refreshToken.AppUserId.ToString());
                if(refreshToken.IsRevoked)
                {
                    /////
                }
                if (!refreshToken.IsActive)
                    throw new InvalidDataException("Invalid token");

                var newRefreshToken = GenerateRefreshToken(ipAddress);

                var existingTokens = new HashSet<string>(user.RefreshTokens.Select(s => s.Token));

                while (existingTokens.Contains(newRefreshToken.Token))
                {
                    newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
                }

                user.RefreshTokens.Add(newRefreshToken);
                // remove old refresh token
                user.RefreshTokens.Remove(refreshToken);
                await _userManager.UpdateAsync(user);

                var accessToken = GenerateAccessToken(user);
                var response = new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken.Token
                };
                return response;               
            }
            return null;
        }
        private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            RevokeRefreshToken(refreshToken, ipAddress/*, "Replaced by new token"*/, newRefreshToken.Token);
            return newRefreshToken;
        }
        private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null,
            string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            //token.ReasonRevoked = reason;
            //token.ReplacedByToken = replacedByToken;
        }
        public async Task<string> RegisterAsync(CreateUserRequest dto)
        {
            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.User);
                return GenerateAccessToken(user);
            }
            else
            {
                throw new BadRequestException(result.Errors.FirstOrDefault().Description.ToString());               
            }
        }

        private async Task removeOldRefreshTokens(AppUser appUser)
        {
            appUser.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.CreatedDate.AddDays(2) <= DateTime.UtcNow);
            await _userManager.UpdateAsync(appUser);
        }


        public string GenerateAccessToken(AppUser user)
        {
            var tokenKey = _configuration["Tokens:Key"];
            var issuer = _configuration["Tokens:Issuer"];
            var audience = _configuration["Tokens:Audience"];
            var expire = _configuration["Tokens:Expire"];
            var roles = _userManager.GetRolesAsync(user).Result;
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

        public JwtSecurityToken? ValidateToken(string? token)
        {
            var tokenKey = _configuration["Tokens:Key"];

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
        public RefreshToken GenerateRefreshToken(string ipAddress)
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

    }
}
