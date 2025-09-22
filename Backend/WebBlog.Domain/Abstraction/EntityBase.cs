namespace WebBlog.Domain.Entities
{
    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; set; }
    }
}
