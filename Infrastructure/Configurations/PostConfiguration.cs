//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WebBlog.Domain.Entities;
//using WebBlog.Infrastructure.Persistance.Constants;

//namespace WebBlog.Infrastructure.Configurations
//{
//    public class PostConfiguration : IEntityTypeConfiguration<Post>
//    {
//        public void Configure(EntityTypeBuilder<Post> builder)
//        {
//            builder.ToTable(TableNames.Post);
//            builder.HasKey(t => t.Id);
//        }
//    }
//}
