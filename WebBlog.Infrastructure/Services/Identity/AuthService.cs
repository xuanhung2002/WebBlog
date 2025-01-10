using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebBlog.Application.Abstraction;
using WebBlog.Application.Dto;
using WebBlog.Application.Exceptions;
using WebBlog.Application.Interfaces;
using WebBlog.Domain.Constant;
using WebBlog.Domain.Entities;
using WebBlog.Infrastructure.Identity;
using WebBlog.Infrastructure.Workers;
using static WebBlog.Application.Dto.AuthDtos;

namespace WebBlog.Infrastructure.Services.Identity
{
    public class AuthService : IAuthService
    {
        private readonly IAppLogger _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAppDBRepository _repository;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IUserCacheService _userCacheService;
        public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration, IAppDBRepository repository, IBackgroundTaskQueue taskQueue, IAppLogger logger, IUserCacheService userCacheService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _repository = repository;
            _taskQueue = taskQueue;
            _logger = logger;
            _userCacheService = userCacheService;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto, string ipAddress)
        {
            var response = new AuthResponseDto();
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
                _logger.Info($"USER {dto.UserName} LOGIN FAILED");
                throw new UnauthorizeException("Username is not existed");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (result.Succeeded)
            {
                var refreshToken = await GenerateRefreshToken(ipAddress);

                user.RefreshTokens.Add(refreshToken);

                await RemoveOldRefreshTokens(user);

                await _userManager.UpdateAsync(user);

                response.AccessToken = await GenerateAccessToken(user);
                response.RefreshToken = refreshToken.Token;
                _logger.Info($"User {user.UserName} logged in");
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
            if (refreshToken != null)
            {
                var user = await _userManager.FindByIdAsync(refreshToken.AppUserId.ToString());
                if (user == null)
                {
                    throw new InvalidDataException($"User with refresh token: {refreshToken.Token} is not existed");
                }
                if (refreshToken.IsRevoked)
                {
                    /////
                }
                if (!refreshToken.IsActive)
                    throw new InvalidDataException("Invalid token");

                var newRefreshToken = await RotateRefreshToken(refreshToken, ipAddress);

                user.RefreshTokens.Add(newRefreshToken);
                // remove old refresh token
                //user.RefreshTokens.Remove(user.RefreshTokens.First(s => s.Token == refreshToken.Token));
                await RemoveOldRefreshTokens(user);

                await _userManager.UpdateAsync(user);

                var accessToken = await GenerateAccessToken(user);
                var response = new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken.Token
                };
                return response;
            }
            return null;
        }
        private async Task<RefreshToken> RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = await GenerateRefreshToken(ipAddress);
            RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }
        private void RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null,
            string? replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.RevokedReason = reason;
            token.RevokedByToken = replacedByToken;
        }
        public async Task<bool> RegisterAsync(CreateUserRequest dto)
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
                await _userCacheService.RefreshUserCache();
                return true;
            }
            else
            {
                throw new BadRequestException(result.Errors.FirstOrDefault().Description.ToString());
            }
        }

        private Task RemoveOldRefreshTokens(AppUser appUser)
        {
            appUser.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.CreatedDate.AddDays(2) <= DateTime.UtcNow);
            return Task.CompletedTask;
        }


        public async Task<string> GenerateAccessToken(AppUser user)
        {
            var tokenKey = _configuration["Tokens:Key"];
            var issuer = _configuration["Tokens:Issuer"];
            var audience = _configuration["Tokens:Audience"];
            var expire = _configuration["Tokens:Expire"];
            var roles = await _userManager.GetRolesAsync(user);
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

        public object ValidateToken(string? token)
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
        public async Task<RefreshToken> GenerateRefreshToken(string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedDate = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
            var existed = await _repository.AnyAsync<RefreshToken>(s => s.Token == refreshToken.Token);
            while (existed)
            {
                return await GenerateRefreshToken(ipAddress);
            }
            return refreshToken;
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            var refreshToken = await _repository.FindForUpdateAsync<RefreshToken>(s => s.Token == token);
            if (!refreshToken.IsActive)
                throw new InvalidDataException("Invalid token");

            RevokeRefreshToken(refreshToken, ipAddress);
        }
    }
}
