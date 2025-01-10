using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Dto;

namespace WebBlog.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUserAsync();
    }
}
