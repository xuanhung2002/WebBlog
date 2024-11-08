﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBlog.Infrastructure.Identity;
using WebBlog.Infrastructure.Persistance.Constants;

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
        }
    }
}
