using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Authorization;
using WebBlog.Application.Dto;
using WebBlog.Application.Interfaces;
using WebBlog.Domain.Constant;

namespace WebBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet]
        [AllowAnonymous]  
        public async Task<IActionResult> Get()
        {
            var posts = await _postService.GetAllAsync();
            return Ok(posts);
        }

        [HttpPost]
        [Access(Roles.Admin, Roles.User)]
        public async Task<IActionResult> Add(PostDto dto)
        {
            var post = await _postService.AddAsync(dto);
            return Ok(post);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var post = await _postService.GetByIdAsync(id);
            return Ok(post);
        }
    }
}
