using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlog.Domain.Entities
{
    public class Comment : EntityAuditBase
    {
        public string Content { get; set; }
        public Guid? ParentCommendId { get; set; }

        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public bool HasChanged { get; set; } = false;
        [Column(TypeName = "nvarchar(max)")]
        public virtual List<CommentUpdateHistory>? Histories { get; set; }
        public virtual List<CommentReaction> Reactions { get; set; }
    }

    public class CommentUpdateHistory
    {
        public string Content { get; set; }
        public DateTimeOffset UpdateTime { get; set; }
    }
}
