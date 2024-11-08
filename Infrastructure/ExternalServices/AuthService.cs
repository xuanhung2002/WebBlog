using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WebBlog.Application.Dtos.ApiRequestDtos;
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
        public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> LoginAsync(LoginDto dto, string ipAddress)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
                throw new UnauthorizeException("Username is not existed");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (result.Succeeded)
            {
                var refreshToken = HttpContextHelper.GenerateRefreshToken(ipAddress);
                user.RefreshTokens.Add(refreshToken);
                removeOldRefreshTokens(user);
                await _userManager.UpdateAsync(user);
                return HttpContextHelper.GenerateToken(user,_userManager, _configuration);
            }
            else
            {
                throw new UnauthorizeException("Password is incorrect");
            }
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
                return HttpContextHelper.GenerateToken(user, _userManager, _configuration);
            }
            else
            {
                throw new BadRequestException(result.Errors.FirstOrDefault().Description.ToString());               
            }
        }

        private Task removeOldRefreshTokens(AppUser appUser)
        {
            appUser.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.CreatedDate.AddDays(7) <= DateTime.UtcNow);
            return Task.CompletedTask;
        }

    }
}
