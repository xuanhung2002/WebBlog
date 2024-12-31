using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WebBlog.Application.Abstraction;
using WebBlog.Domain.Entities;

namespace WebBlog.Infrastructure.Persistances.DataSeeding
{
    public class CategorySeeder
    {
        public static async Task SeedCategories(IServiceProvider serviceProvider)
        {
            var _logger = serviceProvider.GetRequiredService<ILogger>();
            var repository = serviceProvider.GetRequiredService<IAppDBRepository>();
            var isExisted = await repository.AnyAsync<Category>(s => true);
            if (isExisted)
            {
                _logger.Information("Category is exised, do not seed");
                return;
            }
            var buildInCategory = new Category
            {
                Name = "Built-in Category"
            };

            var categoryEntity = await repository.AddAsync(buildInCategory);

            var subCategories = new List<SubCategory>();
            for (int i = 0; i <= 5; i++)
            {
                subCategories.Add(new SubCategory
                {
                    CategoryId = categoryEntity.Id,
                    Name = $"Sub category {i}"
                });
            }

            await repository.AddRangeAsync(subCategories);
        }
    }
}
