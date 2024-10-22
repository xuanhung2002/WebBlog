using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Authorization;
using WebBlog.Domain;

namespace WebBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet]
        [Access(RoleNames.User)]
        public async Task<IActionResult> Get()
        {
            return Ok("dep trai");
        }
    }
}
