using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBlog.Infrastructure.Identity;
using WebBlog.Infrastructure.Persistance.Constants;

namespace WebBlog.Infrastructure.Configurations
{
    public sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable(TableNames.AppRoles);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.RoleCode).HasMaxLength(50).IsRequired(true);

        }
    }
}
