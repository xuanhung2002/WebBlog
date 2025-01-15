using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Common.DataFilters
{
    public enum CCommentFilter
    {
        Default = 0,
        Newest = 1,
        Oldest = 2,
        MostPopular = 3,
    }
}
