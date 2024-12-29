using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebBlog.Domain.Abstraction;

namespace WebBlog.Domain.Entities
{
    public class Comment : EntityAuditBase
    {
        public string Content { get; set; }
        public Guid? ParentCommendId { get; set; }

        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}
