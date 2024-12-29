using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBlog.Infrastructure.Identity;
using WebBlog.Infrastructure.Persistance;

namespace WebBlog.Infrastructure.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable(TableNames.AppUsers);
            builder.HasKey(t => t.Id);

            // each user can have many UserClaims
            builder.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // each user can have many UserLogins
            builder.HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // each user can have many UserTokens
            builder.HasMany(e => e.Tokens)
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();
            
            builder.HasMany(e => e.RefreshTokens)
                .WithOne()
                .HasForeignKey(t => t.AppUserId)
                .IsRequired();

            // User - Post: One to many
            builder.HasMany(e => e.Posts)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // User - Comment: One to many
            builder.HasMany(e => e.Comments)
                .WithOne()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            // User - PostReaction: One to many
            builder.HasMany(e => e.PostReactions)
                .WithOne()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
