using AutoMapper;
using WebBlog.Application.Abstraction;
using WebBlog.Application.Common;
using WebBlog.Application.Dtos.CommentDtos;
using WebBlog.Application.Exceptions;
using WebBlog.Application.Interfaces;
using WebBlog.Domain.Entities;

namespace WebBlog.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IAppDBRepository _repository;
        private readonly IMapper _mapper;
        public CommentService(IAppDBRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CommentDto>?> GetCommentByPostIdAsync(Guid postId)
        {
            var comments = _repository.GetSet<Comment>(s => s.PostId == postId);
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);

            var commentLookup = commentDtos.ToLookup(c => c.ParentCommendId);

            // build subcomment from root comment
            var rootComments = commentLookup[null].ToList();
            foreach (var rootComment in rootComments)
            {
                BuildCommentTree(rootComment, commentLookup);
            }

            return await Task.FromResult(rootComments);
        }

        // recur to build comment tree
        private void BuildCommentTree(CommentDto parentComment, ILookup<Guid?, CommentDto> commentLookup)
        {
            var subComments = commentLookup[parentComment.Id].ToList();
            parentComment.SubComments.AddRange(subComments);

            foreach (var subComment in subComments)
            {
                BuildCommentTree(subComment, commentLookup);
            }
        }

        public async Task<CAddResult> UpdateCommentAsync(CommentDto dto)
        {
            var entity = await _repository.FindForUpdateAsync<Comment>(s => s.Id == dto.Id);

            if(entity.CreatedBy != RuntimeContext.CurrentUser.Id)
            {
                throw new UnauthorizeException("You are not allow to edit this resource");
            }

            if(entity == null)
            {
                throw new BadRequestException("Comment does not exist");
            }
            entity.Content = dto.Content;

            var result = await _repository.UpdateAsync(entity);
            return new CAddResult
            {
                Id = result.Id
            };           
        }

        public async Task<CAddResult> AddAsync(CommentDto dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            var res = await _repository.AddAsync(entity);
            return new CAddResult { Id = res.Id };
        }
    }
}
