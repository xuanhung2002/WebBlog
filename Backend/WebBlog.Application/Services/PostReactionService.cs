using AutoMapper;
using WebBlog.Application.Abstraction;
using WebBlog.Application.Interfaces;
using WebBlog.Domain.Entities;

namespace WebBlog.Application.Services
{
    public class PostReactionService : IPostReactionService
    {
        private readonly IAppDBRepository _repository;
        private readonly IMapper _mapper;
        public PostReactionService(IAppDBRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<PostReactionDto>> GetPostReactionsByPostId(Guid postId)
        {
            var postReactions = await _repository.GetAsync<PostReaction>(s => s.PostId == postId); 
            var res = _mapper.Map<List<PostReactionDto>>(postReactions);
            return res;
        }
    }
}
