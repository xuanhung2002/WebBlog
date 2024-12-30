namespace WebBlog.Application.Dto
{
    public class PostDto : DtoBase
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid SubCategoryId { get; set; }
    }
}
