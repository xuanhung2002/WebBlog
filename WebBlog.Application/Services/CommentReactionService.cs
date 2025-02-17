using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Abstraction;
using WebBlog.Application.Common;
using WebBlog.Application.Exceptions;
using WebBlog.Application.Interfaces;
using WebBlog.Domain.Entities;

namespace WebBlog.Application.Services
{
    public class CommentReactionService : ICommentReactionService
    {
        private readonly IAppDBRepository _repository;
        private readonly IMapper _mapper;
        public CommentReactionService(IAppDBRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CAddResult> AddCommentReactionAsync(CommentReactionDto dto)
        {
            var entity = new CommentReaction
            {
                CommentId = dto.CommentId,
                Type = dto.Type,
                UserId = dto.UserId ?? RuntimeContext.CurrentUser.Id,
            };
            entity = await _repository.AddAsync(entity);

            return new CAddResult
            {
                Id = entity.Id
            };

        }

        public Task DeleteCommentReactionAsync(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public async Task<CAddResult> UpdateCommentReactionAsync(CommentReactionDto dto)
        {
            var entity = await _repository.FindForUpdateAsync<CommentReaction>(s => s.Id == dto.Id);
            if (entity == null)
            {
                throw new BadRequestException("Entity is not existed");
            }
            if(entity.UserId != RuntimeContext.CurrentUser?.Id)
            {
                throw new UnauthorizeException("You are not allowed to update this comment");
            }
            
            entity.Type = dto.Type;
            
            entity = await _repository.UpdateAsync(entity);
            return new CAddResult { Id = entity.Id };
        }
    }
}
