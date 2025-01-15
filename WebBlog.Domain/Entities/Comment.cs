namespace WebBlog.Domain.Entities
{
    public class Comment : EntityAuditBase
    {
        public string Content { get; set; }
        public Guid? ParentCommendId { get; set; }

        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public virtual List<CommentReaction> Reactions { get; set; }
    }
}
