using WebBlog.Domain.Enums;

namespace WebBlog.Application
{
    public class PostReactionDto : DtoAuditBase
    {
        public Guid PostId { get; set; }
        public CPostReactionType Type { get; set; }
        public Guid UserId { get; set; }
    }
}
