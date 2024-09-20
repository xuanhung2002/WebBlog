using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Domain.Abstraction;
using WebBlog.Domain.Abstraction.Entities;

namespace WebBlog.Infrastructure.Identity
{
    public class Function : EntityBase<int>, IDateTracking
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
