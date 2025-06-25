using static WebBlog.Application.AuthDtos;

namespace WebBlog.Application
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
