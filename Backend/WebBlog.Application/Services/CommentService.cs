using AutoMapper;
using WebBlog.Application.Abstraction;
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
            var comments = _repository.GetSet<Comment>(s => s.PostId == postId && s.IsDeleted != true);
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
            if(subComments.Any())
            {
                parentComment.SubComments.AddRange(subComments);

                foreach (var subComment in subComments)
                {
                    BuildCommentTree(subComment, commentLookup);
                }
            }
        }

        public async Task<CAddResult> UpdateCommentAsync(CommentDto dto)
        {
            var entity = await _repository.FindForUpdateAsync<Comment>(s => s.Id == dto.Id);

            if (entity == null)
            {
                throw new BadRequestException("Comment does not exist");
            }

            if (entity.CreatedBy != RuntimeContext.CurrentUser.Id)
            {
                throw new UnauthorizeException("You are not allow to edit this resource");
            }
            entity.Content = dto.Content;
            entity.HasChanged = true;
            entity.Histories.Add(new CommentUpdateHistory
            {
                Content = entity.Content,
                UpdateTime = entity.ModifiedDate.Value
            });

            var result = await _repository.UpdateAsync(entity);
            return new CAddResult
            {
                Id = result.Id
            };           
        }

        public async Task<CAddResult> AddAsync(CommentDto dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            entity.UserId = !dto.UserId.Equals(Guid.Empty) ? dto.UserId : RuntimeContext.CurrentUser.Id;
            var res = await _repository.AddAsync(entity);
            return new CAddResult { Id = res.Id };
        }

        public async Task<TableInfo<CommentDto>> GetCommentByPostWithPaging(GetCommentsRequest request)
        {
            List<Comment> commentEntities = new List<Comment>();
            if(request.CommentFilter != null)
            {
                switch(request.CommentFilter)
                {
                    case CCommentFilter.Newest:
                        commentEntities = await _repository.GetWithOrderAsync<Comment>(orderBy: s => s.OrderByDescending(x => x.CreatedDate));
                        break;
                    case CCommentFilter.Oldest:
                        commentEntities = await _repository.GetWithOrderAsync<Comment>(orderBy: s => s.OrderBy(x => x.CreatedDate));
                        break;
                    case CCommentFilter.MostPopular:
                        commentEntities = await _repository.GetWithOrderAsync<Comment>(orderBy: s => s.OrderBy(x => x.Reactions.Count));
                        break;
                    default:
                        commentEntities = await _repository.GetAsync<Comment>();
                        break;
                }
            }
            var totalCount = commentEntities.Count();
            // apply paging
            commentEntities = commentEntities.Skip(request.Parameter.PageIndex - 1).Take(request.Parameter.PageSize).ToList();
            int pageCount = (int)Math.Ceiling(totalCount / (double)request.Parameter.PageSize);
            var dtos = _mapper.Map<List<CommentDto>>(commentEntities);
            return new TableInfo<CommentDto>
            {
                Items = dtos,
                ItemsCount = totalCount,
                PageCount = pageCount
            };
        }
    }
}
