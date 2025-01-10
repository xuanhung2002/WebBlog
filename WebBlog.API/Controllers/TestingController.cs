using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Application.Interfaces;

namespace WebBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly IUserCacheService _userCacheService;
        public TestingController(IUserCacheService userCacheService)
        {
            _userCacheService = userCacheService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userCacheService.GetUserFromCacheById(id);
            return Ok(user);
        }
        [HttpPost("getbyids")]
        public async Task<IActionResult> GetByIds(List<Guid> ids)
        {
            var users = await _userCacheService.GetUserFromCacheByIds(ids);
            return Ok(users);
        }
    }
}
