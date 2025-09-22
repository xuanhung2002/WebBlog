using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Authorization;
using WebBlog.Application;
using WebBlog.Application.Interfaces;
using WebBlog.Domain.Constant;

namespace WebBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // need modify => limit number of comment
        [HttpGet("getbypostid/{postId}")]
        public async Task<IActionResult> GetCommentByPostId(Guid postId)
        {
            var res = await _commentService.GetCommentByPostIdAsync(postId);
            return Ok(res);
        }

        [HttpPut]
        [Access(Roles.Admin, Roles.User)]
        public async Task<IActionResult> UpdateComment(CommentDto dto)
        {
            var res = await _commentService.UpdateCommentAsync(dto);
            return Ok(res);
        }

        [HttpPost]
        [Access(Roles.Admin, Roles.User)]
        public async Task<IActionResult> AddCommewnt(CommentDto dto)
        {
            var res = await _commentService.AddAsync(dto);
            return Ok(res);
        }

        [HttpPost("getbypost")]
        public async Task<IActionResult> GetCommentByPost(GetCommentsRequest request)
        {
            var res = await _commentService.GetCommentByPostWithPaging(request);
            return Ok(res);
        }
    }
}
