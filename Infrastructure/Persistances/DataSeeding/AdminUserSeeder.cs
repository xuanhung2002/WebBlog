using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WebBlog.Application.Exceptions;
using WebBlog.Domain.Constant;
using WebBlog.Infrastructure.Identity;

namespace WebBlog.Infrastructure.Persistances
{
    public class AdminUserSeeder
    {
        public static async Task SeedAdmin(UserManager<AppUser> userManager, IServiceProvider serviceProvider)
        {
            var existed = await userManager.FindByNameAsync("admin");
            if (existed != null)
            {
                var logger = serviceProvider.GetRequiredService<ILogger>();
                logger.Debug("admin account is already existed");
                return;
            }
            var password = "Admin@123";
            var user = new AppUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FirstName = "admin",
                LastName = "admin",
            };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
            else
            {
                throw new BadRequestException(result.Errors.FirstOrDefault().Description.ToString());
            }
        }
    }
}
