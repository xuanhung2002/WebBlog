using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBlog.Domain.Entities;
using WebBlog.Infrastructure.Persistance;

namespace WebBlog.Infrastructure.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable(TableNames.Post);
            builder.HasKey(t => t.Id);

            builder.HasMany(s => s.Comments)
                .WithOne()
                .HasForeignKey(s => s.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.Reactions)
                .WithOne()
                .HasForeignKey(s => s.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
