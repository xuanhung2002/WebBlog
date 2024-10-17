using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Dtos
{
    public class DtoBase<TId>
    {
        public TId Id { get; set; }
    }
}
