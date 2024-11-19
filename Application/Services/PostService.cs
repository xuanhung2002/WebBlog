using WebBlog.Application.Abstraction;
using WebBlog.Application.Dtos;
using WebBlog.Application.Interface;
using WebBlog.Domain.Entities;

namespace WebBlog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IAppDBRepository _repository;
        public PostService(IAppDBRepository repository)
        {
            _repository = repository;
        }
        public async Task<Post> AddAsync(PostDto dto)
        {
            var newPost = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
            };
            return await _repository.AddAsync(newPost);
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
