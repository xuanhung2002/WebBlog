using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Common;
using WebBlog.Application.Dtos.CommentDtos;

namespace WebBlog.Application.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentDto>?> GetCommentByPostIdAsync(Guid postId);
        Task<CAddResult> UpdateCommentAsync(CommentDto dto);

        Task<CAddResult> AddAsync(CommentDto dto);
    }
}
