namespace WebBlog.Domain.Entities
{
    public class SubCategory : EntityAuditBase
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        //public Category Category { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}
