using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Infrastructure.Identity;
using WebBlog.Infrastructure.Persistance.Constants;

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
