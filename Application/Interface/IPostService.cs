using WebBlog.Application.Dtos;
using WebBlog.Domain.Entities;

namespace WebBlog.Application.Interface
{
    public interface IPostService
    {
        public Task<List<PostDto>> GetAllAsync();
        public Task<Post> AddAsync(PostDto dto);
    }
}
