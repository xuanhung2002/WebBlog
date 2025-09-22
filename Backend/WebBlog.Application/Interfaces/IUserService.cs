namespace WebBlog.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUserAsync();
        Task<CAddResult> UpdateUserAsync(UserDto user);
    }
}
