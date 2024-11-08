using WebBlog.Application.Abstraction;
using WebBlog.Application.Dtos;
using WebBlog.Application.Interface;
using WebBlog.Domain.Entities;
using WebBlog.Infrastructure.UoWMultiContext;

namespace WebBlog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IAppDBRepository _repository;
        private readonly IAppDbContextUnitOfWork _unitOfWork;
        public PostService(IAppDBRepository repository, IAppDbContextUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Post> AddAsync(PostDto dto)
        {
            var newPost = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
            };
            _repository.Add(newPost);
            await _unitOfWork.SaveChangeAsync();
            return newPost;

        }

        public async Task<List<PostDto>> GetAllAsync()
        {
            var posts = _repository.FindAll<Post>().ToList();
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
