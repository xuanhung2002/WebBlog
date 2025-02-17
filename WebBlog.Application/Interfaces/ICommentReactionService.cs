using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Common;

namespace WebBlog.Application.Interfaces
{
    public interface ICommentReactionService
    {
        Task<CAddResult> AddCommentReactionAsync(CommentReactionDto dto);
        Task<CAddResult> UpdateCommentReactionAsync(CommentReactionDto dto);
        Task DeleteCommentReactionAsync(Guid commentId);


    }
}
