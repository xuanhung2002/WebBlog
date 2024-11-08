using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Domain.Abstraction;
using WebBlog.Domain.Abstraction.Entities;

namespace WebBlog.Infrastructure.Identity
{
    public class RefreshToken : EntityAuditBase
    {
        public Guid AppUserId { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Expires { get; set; }
        public string CreatedByIp { get; set; }
        public DateTimeOffset? Revoked { get; set; }
        public string? RevokedByIp { get; set; }
        public string? RevokedByToken { get; set; }
        public string? RevokedReason { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => Revoked == null && !IsExpired;
        
    }
}
