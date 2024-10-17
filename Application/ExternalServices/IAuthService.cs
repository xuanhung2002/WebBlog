using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Dtos.ApiRequestDtos;
using static WebBlog.Application.Dtos.ApiRequestDtos.AuthDtos;

namespace WebBlog.Application.ExternalServices
{
    public interface IAuthService
    {
        public Task<string> LoginAsync(LoginDto dto);
        public Task<string> RegisterAsync(CreateUserRequest dto);
    }
}
