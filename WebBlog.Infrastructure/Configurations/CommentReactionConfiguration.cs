using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBlog.Domain.Entities;
using WebBlog.Infrastructure.Persistances;

namespace WebBlog.Infrastructure.Configurations
{
    public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
    {
        public void Configure(EntityTypeBuilder<CommentReaction> builder)
        {
            builder.ToTable(TableNames.CommentReaction);
            builder.HasKey(t => t.Id);
        }
    }
}
