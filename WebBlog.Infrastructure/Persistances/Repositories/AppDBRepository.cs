using WebBlog.Application.Abstraction;
using WebBlog.Application.ExternalServices;

namespace WebBlog.Infrastructure.Persistances
{
    public class AppDBRepository<AppDBContext> : RepositoryBaseDbContext<AppDbContext>, IAppDBRepository
    {
        public AppDBRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
