using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Dtos.ApiRequestDtos;
using WebBlog.Application.Dtos.AuthDtos;
using static WebBlog.Application.Dtos.ApiRequestDtos.AuthDtos;

namespace WebBlog.Application.ExternalServices
{
    public interface IAuthService
    {
        public Task<AuthResponseDto> LoginAsync(LoginDto dto, string ipAddress);
        public Task<AuthResponseDto> RefreshTokenAsync(string token, string ipAddress);
        public Task<string> RegisterAsync(CreateUserRequest dto);
        public object ValidateToken(string? token);
        public Task RevokeToken(string token, string ipAddress);
    }
}
