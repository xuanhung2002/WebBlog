namespace WebBlog.Domain.Entities
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public virtual List<SubCategory> SubCategories { get; set; }
    }
}
