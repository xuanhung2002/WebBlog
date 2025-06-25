namespace WebBlog.Application.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentDto>?> GetCommentByPostIdAsync(Guid postId);
        Task<CAddResult> UpdateCommentAsync(CommentDto dto);

        Task<CAddResult> AddAsync(CommentDto dto);

        Task<TableInfo<CommentDto>> GetCommentByPostWithPaging(GetCommentsRequest request);
    }
}
