using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Dto;
using WebBlog.Application.Dtos;
using WebBlog.Domain.Enums;

namespace WebBlog.Application
{
    public class PostReactionDto : DtoAuditBase
    {
        public Guid PostId { get; set; }
        public CPostReactionType Type { get; set; }
        public Guid UserId { get; set; }
    }
}
