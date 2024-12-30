using WebBlog.Application.Common;

namespace WebBlog.Application
{
    public static class RuntimeContext
    {
        public static CUserBase? CurrentUser { get; set; }
    }
}
