using WebBlog.Application;
using WebBlog.Application.Common;
using WebBlog.Application.ExternalServices;

namespace WebBlog.Infrastructure.ExternalServices
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService()
        {
        }

        public CUserBase? GetCurrentUser()
        {
            return RuntimeContext.CurrentUser == null ? null : new CUserBase
            {
                Id = RuntimeContext.CurrentUser.Id,
            };
        }
    }
}
