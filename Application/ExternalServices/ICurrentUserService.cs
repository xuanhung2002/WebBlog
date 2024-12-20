using WebBlog.Application.Common;

namespace WebBlog.Application.ExternalServices
{
    public interface ICurrentUserService
    {
        CUserBase? GetCurrentUser();
    }
}
