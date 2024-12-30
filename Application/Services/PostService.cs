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
        public PostService(IAppDBRepository repository)
        {
            _repository = repository;
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
            var dtos = new List<PostDto>();
            foreach (var post in posts)
            {
                dtos.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                });
            }
            return dtos;
        }

    }
}
