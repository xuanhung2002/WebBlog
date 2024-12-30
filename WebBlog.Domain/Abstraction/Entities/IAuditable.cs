namespace WebBlog.Domain.Entities
{
    public interface IAuditable : IDateTracking, IUserTracking, ISoftDelete
    {
    }
}
