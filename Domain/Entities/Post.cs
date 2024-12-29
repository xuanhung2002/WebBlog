using WebBlog.Domain.Abstraction;

namespace WebBlog.Domain.Entities
{
    public class Post : EntityAuditBase
    {   
        public Guid? UserId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

        public Guid SubCategoryId { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<PostReaction> Reactions { get; set; }
    }
}
