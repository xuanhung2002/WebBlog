using Microsoft.AspNetCore.Identity;
using WebBlog.Domain;
using WebBlog.Domain.Constant;
using WebBlog.Infrastructure.Identity;


namespace WebBlog.Infrastructure.Persistances
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            string[] roles = { Roles.Admin, Roles.User, Roles.Guest };
            foreach (string role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    var newRole = new AppRole
                    {
                        Name = role,
                        RoleCode = role.ToUpper(),
                        Description = $"{role} role"
                    };
                    await roleManager.CreateAsync(newRole);
                }
            }
        }
    }
}
