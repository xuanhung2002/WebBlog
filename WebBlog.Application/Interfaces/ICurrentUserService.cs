using WebBlog.Application.Common;

namespace WebBlog.Application.Interfaces
{
    public interface ICurrentUserService
    {
        CUserBase? GetCurrentUser();
    }
}
