using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBlog.Domain.Entities;
using WebBlog.Infrastructure.Persistances;

namespace WebBlog.Infrastructure.Configurations
{
    public class PostReactionConfiguration : IEntityTypeConfiguration<PostReaction>
    {
        public void Configure(EntityTypeBuilder<PostReaction> builder)
        {
            builder.ToTable(TableNames.PostReaction);
            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.PostId).HasDatabaseName("Index_PostReaction_PostId");
        }
    }
}
