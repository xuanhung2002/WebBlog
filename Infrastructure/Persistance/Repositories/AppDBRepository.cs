using WebBlog.Application.Abstraction;

namespace WebBlog.Infrastructure.Persistance.Repositories
{
    public class AppDBRepository<AppDBContext> : RepositoryBaseDbContext<AppDbContext>, IAppDBRepository
    {
        public AppDBRepository(AppDbContext context) : base(context)
        {
        }
    }
}
