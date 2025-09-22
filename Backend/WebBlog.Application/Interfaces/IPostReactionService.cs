using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Interfaces
{
    public interface IPostReactionService
    {
        Task<List<PostReactionDto>> GetPostReactionsByPostId(Guid postId);
    }
}
