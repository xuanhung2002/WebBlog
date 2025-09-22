using WebBlog.Domain.Enums;

namespace WebBlog.Application
{
    public class CommentReactionDto :DtoAuditBase
    {
        public Guid CommentId { get; set; }
        public CCommentReactionType Type { get; set; }
        public Guid? UserId { get; set; }
    }
}
