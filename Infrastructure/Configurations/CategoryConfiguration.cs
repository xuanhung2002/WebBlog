using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBlog.Domain.Entities;
using WebBlog.Infrastructure.Persistance;

namespace WebBlog.Infrastructure
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(TableNames.Category);
            builder.HasKey(c => c.Id);

            builder.HasMany(s => s.SubCategories)
                .WithOne()
                .HasForeignKey(s => s.CategoryId);
        }
    }
}
