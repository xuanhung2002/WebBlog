namespace WebBlog.Application.Interfaces
{
    public interface ICommentReactionService
    {
        Task<CAddResult> AddCommentReactionAsync(CommentReactionDto dto);
        Task<CAddResult> UpdateCommentReactionAsync(CommentReactionDto dto);
        Task DeleteCommentReactionAsync(Guid commentId);


    }
}
