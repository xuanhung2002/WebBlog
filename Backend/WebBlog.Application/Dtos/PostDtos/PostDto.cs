namespace WebBlog.Application
{
    public class PostDto : DtoAuditBase
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool? IsAnonymous { get; set; }
        public Guid UserId { get; set; }
        public Guid SubCategoryId { get; set; }
    }
}
