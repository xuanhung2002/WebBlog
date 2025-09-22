using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBlog.Domain.Entities;
using WebBlog.Infrastructure.Identity;

namespace WebBlog.Infrastructure
{
    public sealed class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<AppUser>().HasQueryFilter(x => !x.IsDeleted);
            //builder.Entity<AppUser>().HasIndex(x => x.IsDeleted).HasFilter("is_delete = 0");
            // Global filter for soft delete pattern
            //var softDeleteEntites = typeof(ISoftDelete).Assembly.GetTypes()
            //    .Where(t => typeof(ISoftDelete).IsAssignableFrom(t)
            //    && t.IsClass);

            //foreach (var softDeleteEntity in softDeleteEntites)
            //{
            //    builder.Entity(softDeleteEntity).HasQueryFilter(GenerateQueryFilterLambda(softDeleteEntity));
            //}

            builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        }

        //private LambdaExpression? GenerateQueryFilterLambda(Type type)
        //{
        //    var parameter = Expression.Parameter(type, "w");
        //    var falseConstantValue = Expression.Constant(false);
        //    var propertiesAccess = Expression.PropertyOrField(parameter, nameof(ISoftDelete.IsDeleted));
        //    var equalExpression = Expression.Equal(propertiesAccess, falseConstantValue);
        //    var lambda = Expression.Lambda(equalExpression, parameter);
        //    return lambda;
        //}

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        //

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<Category> Categoroies { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

    }
}
