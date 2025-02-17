using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebBlog.Application.Abstraction;
using WebBlog.Application.Common;
using WebBlog.Application.Dto;
using WebBlog.Application.Interfaces;
using WebBlog.Infrastructure.Identity;

namespace WebBlog.Infrastructure.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly IAppDBRepository _appDBRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserCacheService _userCacheService;
        public UserService(IAppDBRepository appDBRepository, UserManager<AppUser> userManager, IMapper mapper, IUserCacheService userCacheService)
        {
            _appDBRepository = appDBRepository;
            _userManager = userManager;
            _mapper = mapper;
            _userCacheService = userCacheService;
        }
        public async Task<List<UserDto>> GetAllUserAsync()
        {
            var users = _userManager.Users.ToList();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return userDtos;
        }

        public async Task<CAddResult> UpdateUserAsync(UserDto user)
        {
            var userEntity = await _userManager.FindByIdAsync(user.Id.ToString());
            if (userEntity == null)
            {
                return new CAddResult();
            }

            userEntity = _mapper.Map<AppUser>(user);
            var res = await _userManager.UpdateAsync(userEntity);
            await _userCacheService.RefreshUserCache(userEntity.Id);
            return new CAddResult
            {
                Id = userEntity.Id,
            };
           
        }
    }
}
