using WebBlog.Application.Common.DataFilters;
using WebBlog.Application.Common.Paging;

namespace WebBlog.Application.Dtos.CommentDtos
{
    public class GetCommentsRequest
    {   
        public Guid PostId { get; set;}
        public CCommentFilter CommentFilter { get; set; }
        public TablePageParameter Parameter { get; set; }

    }
}
