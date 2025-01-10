using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebBlog.Application.Abstraction;
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
        public UserService(IAppDBRepository appDBRepository, UserManager<AppUser> userManager, IMapper mapper)
        {
            _appDBRepository = appDBRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<List<UserDto>> GetAllUserAsync()
        {
            var users = _userManager.Users.ToList();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return userDtos;
        }
    }
}
