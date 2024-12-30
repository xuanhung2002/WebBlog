using Microsoft.Extensions.DependencyInjection;
using WebBlog.Application.Abstraction;

namespace WebBlog.Infrastructure.Persistances.DataSeeding
{
    public class CategorySeeder
    {
        public static async Task SeedCategories(IServiceProvider serviceProvider)
        {
            var repository = serviceProvider.GetService<IAppDBRepository>();

        }
    }
}
