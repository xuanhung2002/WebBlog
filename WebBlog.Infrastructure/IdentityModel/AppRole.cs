using Microsoft.AspNetCore.Identity;
using WebBlog.Domain.Entities;

namespace WebBlog.Infrastructure.Identity
{
    public class AppRole : IdentityRole<Guid>, IAuditable
    {
        public string Description { get; set; }
        public string RoleCode { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
