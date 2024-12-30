using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBlog.Domain.Entities;
using WebBlog.Infrastructure.Persistances;

namespace WebBlog.Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(TableNames.Comment);
            builder.HasKey(t => t.Id);
        }
    }
}
