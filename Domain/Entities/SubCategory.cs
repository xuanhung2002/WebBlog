using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Domain.Abstraction;

namespace WebBlog.Domain.Entities
{
    public class SubCategory : EntityAuditBase
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}
