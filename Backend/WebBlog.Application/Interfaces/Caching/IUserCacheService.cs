namespace WebBlog.Application.Interfaces
{
    public interface IUserCacheService
    {
        Task<UserDto> GetUserFromCacheById(Guid id);
        Task<List<UserDto>> GetUserFromCacheByIds(List<Guid> ids);
        Task RefreshUserCache(Guid? id = null);
    }
}
