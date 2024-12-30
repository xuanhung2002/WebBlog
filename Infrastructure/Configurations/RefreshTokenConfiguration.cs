using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBlog.Infrastructure.Identity;
using WebBlog.Infrastructure.Persistances;

namespace WebBlog.Infrastructure.Configurations
{
    public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(TableNames.RefreshToken);
            builder.HasKey(x => x.Id);            
        }
    }
}
