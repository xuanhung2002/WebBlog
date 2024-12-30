using AutoMapper;
using WebBlog.Application.Abstraction;
using WebBlog.Application.Common;
using WebBlog.Application.Dto;
using WebBlog.Application.Interfaces;
using WebBlog.Domain.Entities;

namespace WebBlog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IAppDBRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<PostService> _logger;
        public PostService(IAppDBRepository repository, IAppLogger<PostService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<CAddResult> AddAsync(PostDto dto)
        {
            var post = _mapper.Map<Post>(dto);
            post.UserId = RuntimeContext.CurrentUser?.Id;
            post = await _repository.AddAsync(post);
            return new CAddResult
            {
                Id = post.Id,
            };
        }

        public async Task<List<PostDto>> GetAllAsync()
        {
            var posts = await _repository.GetAsync<Post>();
            var dtos = _mapper.Map<List<PostDto>>(posts);     
            return dtos;
        }

        public async Task<PostDto> GetByIdAsync(Guid id)
        {
            var post = await _repository.FindAsync<Post>(s => s.Id == id);
            if (post == null)
            {
                _logger.Warning($"Post id: {id} is not existed");
                return null;
            }

            var postDto = _mapper.Map<PostDto>(post);
            return postDto;
        }
    }
}
