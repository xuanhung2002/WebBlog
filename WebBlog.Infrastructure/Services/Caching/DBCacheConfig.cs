using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Domain.Entities;

namespace WebBlog.Infrastructure.Services.Caching
{
    public class DBCacheConfig
    {
        public static readonly HashSet<Type> cacheTypes = new HashSet<Type>
        {
            typeof(Post),
            typeof(PostReaction),
            typeof(Comment),
            typeof(CommentReaction),
        };
    }
}
