using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Authorization;
using WebBlog.Application.Dtos;
using WebBlog.Application.Interface;
using WebBlog.Domain;

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
            var posts = _postService.GetAllAsync();
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PostDto dto)
        {
            var post = await _postService.AddAsync(dto);
            return Ok(post);
        }
    }
}
