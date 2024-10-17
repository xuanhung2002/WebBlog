using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Dtos.ApiRequestDtos
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName + LastName}";
        public string Email { get; set; }
    }
}
