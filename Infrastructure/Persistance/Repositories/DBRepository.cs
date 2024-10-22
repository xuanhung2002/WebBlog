using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Domain.Abstraction;

namespace WebBlog.Infrastructure.Persistance.Repositories
{
    public class DBRepository<T> : RepositoryBaseDbContext<ApplicationDbContext, T> where T : EntityAuditBase
    {
        public DBRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
