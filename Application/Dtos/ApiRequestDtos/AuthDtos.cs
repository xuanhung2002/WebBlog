using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Dtos.ApiRequestDtos
{
    public class AuthDtos
    {
        public class LoginDto
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
