namespace WebBlog.Application
{
    public class SubcategoryDto : DtoBase
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        //public Category Category { get; set; }
        public List<PostDto> Posts { get; set; }
    }
}
