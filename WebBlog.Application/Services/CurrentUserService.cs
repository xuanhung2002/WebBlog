using WebBlog.Application.Common;
using WebBlog.Application.Interfaces;

namespace WebBlog.Application.Services
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
