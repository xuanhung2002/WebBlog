using Microsoft.AspNetCore.Identity;
using WebBlog.Domain;
using WebBlog.Infrastructure.Identity;


namespace WebBlog.Infrastructure.Persistance.DataSeeding
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            string[] roleNames = { RoleNames.Admin, RoleNames.User, RoleNames.Guest };
            foreach (string roleName in roleNames)
            {
                if(!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new AppRole
                    {
                        Name = roleName,
                        RoleCode = roleName.ToUpper(),
                        Description = $"{roleName} role"
                    };
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
