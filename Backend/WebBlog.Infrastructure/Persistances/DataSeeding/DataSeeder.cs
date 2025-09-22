using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WebBlog.Application.Interfaces;
using WebBlog.Infrastructure.Identity;

namespace WebBlog.Infrastructure.Persistances.DataSeeding
{
    public static class DataSeeder
    {
        public static async Task SeedData(IServiceProvider services)
        {   var _logger = services.GetRequiredService<IAppLogger>();
            _logger.Info("Start SEED app data");
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            await RoleSeeder.SeedRolesAsync(roleManager);
            await AdminUserSeeder.SeedAdmin(userManager, services);
            await CategorySeeder.SeedCategories(services);
            _logger.Info("End SEED app data");
        }
    }
}
