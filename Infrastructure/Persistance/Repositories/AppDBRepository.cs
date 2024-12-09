using WebBlog.Application.Abstraction;
using WebBlog.Application.ExternalServices;

namespace WebBlog.Infrastructure.Persistance.Repositories
{
    public class AppDBRepository<AppDBContext> : RepositoryBaseDbContext<AppDbContext>, IAppDBRepository
    {
        public AppDBRepository(AppDbContext context, ICacheService cacheService) : base(context, cacheService)
        {
        }
    }
}
