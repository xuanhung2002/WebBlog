using WebBlog.Domain.Abstraction;

namespace WebBlog.Domain.Entities
{
    public class Post : EntityAuditBase
    {
        public Guid? UserId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

    }
}
