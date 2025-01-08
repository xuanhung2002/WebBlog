using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Abstraction;
using WebBlog.Application.Dto;
using WebBlog.Application.Interfaces;
using WebBlog.Application.Interfaces.Caching;
using WebBlog.Infrastructure.Identity;

namespace WebBlog.Infrastructure.Services.Caching
{
    public class UserCacheService : IUserCacheService
    {
        private static readonly string UserCacheKey = "User";
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IAppDBRepository _repository;
        private readonly IMapper _mapper;
        public UserCacheService(IMemoryCacheService memoryCacheService, IAppDBRepository repository, IMapper mapper)
        {
            _memoryCacheService = memoryCacheService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserFromCacheById(Guid id)
        {   
            var key = $"{UserCacheKey}_{id}";
            var user = _memoryCacheService.Get<UserDto>(key);
            if(user == null)
            {
                var userEntity = await _repository.FindAsync<AppUser>(s => s.Id == id);
                if(userEntity != null)
                {
                    user = _mapper.Map<UserDto>(userEntity);
                    _memoryCacheService.Set(key, user);
                }
            }
            return user;
        }

        public async Task<List<UserDto>> GetUserFromCacheByIds(List<Guid> ids)
        {
            var users = _memoryCacheService.Get<List<UserDto>>(UserCacheKey);
            if(users == null)
            {
                var entities = await _repository.GetAsync<AppUser>(s => true);
                users = _mapper.Map<List<UserDto>>(entities);
                _memoryCacheService.Set(UserCacheKey, users);
            }
            var res = users.Where(s => ids.Contains(s.Id)).ToList();
            return res;
        }

        public async Task RefreshUserCache(Guid? id = null)
        {
            if (id != null)
            {
                var key = $"{UserCacheKey}_{id}";
                _memoryCacheService.Remove(key);

                var userEntity = await _repository.FindAsync<AppUser>(s => s.Id == id);
                if (userEntity != null)
                {
                    var userDto = _mapper.Map<UserDto>(userEntity);
                    _memoryCacheService.Set(key, userDto, TimeSpan.FromHours(1)); 
                }
            }

            _memoryCacheService.Remove(UserCacheKey);

            var users = await _repository.GetAsync<AppUser>(s => true); 
            var userDtos = _mapper.Map<List<UserDto>>(users);

            _memoryCacheService.Set(UserCacheKey, userDtos, TimeSpan.FromHours(1)); 
        }

    }
}
