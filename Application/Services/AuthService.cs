using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Dtos.ApiRequestDtos;
using WebBlog.Application.ExternalServices;

namespace WebBlog.Application.Services
{
    public class AuthService : IAuthService
    {
        public AuthService()
        {
            
        }
        public Task<string> LoginAsync(AuthDtos.LoginDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> RegisterAsync(CreateUserRequest dto)
        {
            throw new NotImplementedException();
        }
    }
}
