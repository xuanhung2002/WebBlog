using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Domain.Abstraction;

namespace WebBlog.Domain.Entities
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public virtual List<SubCategory> SubCategories { get; set; }
    }
}
