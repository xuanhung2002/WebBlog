using WebBlog.Domain.Entities;

namespace WebBlog.Application
{
    public class CategoryDto : DtoBase
    {
        public string Name { get; set; }
        public List<SubcategoryDto> SubCategories { get; set; }
    }
}
