using WebBlog.Application.Dto;
using static WebBlog.Application.Dto.AuthDtos;

namespace WebBlog.Application.ExternalServices
{
    public interface IAuthService
    {
        public Task<AuthResponseDto> LoginAsync(LoginDto dto, string ipAddress);
        public Task<AuthResponseDto> RefreshTokenAsync(string token, string ipAddress);
        public Task<bool> RegisterAsync(CreateUserRequest dto);
        public object ValidateToken(string? token);
        public Task RevokeToken(string token, string ipAddress);
    }
}
