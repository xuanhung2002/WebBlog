namespace WebBlog.Application.Dtos.CommentDtos
{
    public class CommentDto : DtoAuditBase
    {
        public string Content { get; set; }
        public Guid? ParentCommendId { get; set; }

        public List<CommentDto>? SubComments { get; set; } = new List<CommentDto>();
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}
