using Microsoft.AspNetCore.Identity;
using WebBlog.Domain.Entities;

namespace WebBlog.Infrastructure.Identity
{
    public class AppUser : IdentityUser<Guid>, IAuditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? FullName { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public virtual List<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
        public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
        public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }

        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public virtual List<Post> Posts { get;} = new List<Post>();
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        public virtual List<PostReaction> PostReactions { get; set; }
    }
}
