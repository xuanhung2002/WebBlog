
using WebBlog.Domain.Enums;

namespace WebBlog.Domain.Entities
{
    public class PostReaction : EntityBase
    {
        public CPostReactionType Type { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}
