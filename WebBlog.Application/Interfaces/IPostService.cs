using WebBlog.Application.Common;
using WebBlog.Application.Dto;

namespace WebBlog.Application.Interfaces
{
    public interface IPostService
    {
        public Task<List<PostDto>> GetAllAsync();
        public Task<CAddResult> AddAsync(PostDto dto);
        public Task<PostDto> GetByIdAsync(Guid id);
        public Task<List<PostDto>> GetRecentPostAsync(int count);

    }
}
