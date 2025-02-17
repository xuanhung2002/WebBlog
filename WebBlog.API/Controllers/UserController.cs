using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Authorization;
using WebBlog.Application.Dto;
using WebBlog.Application.Interfaces;
using WebBlog.Domain.Constant;

namespace WebBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserCacheService _userCacheService;
        public UserController(IUserService userService, IUserCacheService userCacheService)
        {
            _userService = userService;
            _userCacheService = userCacheService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUserAsync();
            return Ok(users);
        }
        [HttpPost("getfromcachebyids")]

        public async Task<IActionResult> GetAllFromCacheByIds(List<Guid> ids)
        {
            var user = await _userCacheService.GetUserFromCacheByIds(ids);
            return Ok(user);
        }

        [HttpGet("getfromcache/{id}")]
        public async Task<IActionResult> GetAllFromCacheById(Guid id)
        {
            var user = await _userCacheService.GetUserFromCacheById(id);
            return Ok(user);
        }
        [HttpPost]
        [Access(Roles.User)]
        public async Task<IActionResult> UpdateUser(UserDto dto)
        {
            var res = await _userService.UpdateUserAsync(dto);
            return Ok(res);
        }
    }
}
