using WebBlog.Domain.Enums;

namespace WebBlog.Domain.Entities
{
    public class CommentReaction : EntityBase
    {
        public CCommentReactionType Type { get; set; }
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
    }
}
