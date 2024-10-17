using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Domain.Abstraction.Entities;

namespace WebBlog.Domain.Abstraction
{
    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; set; }
    }
}
