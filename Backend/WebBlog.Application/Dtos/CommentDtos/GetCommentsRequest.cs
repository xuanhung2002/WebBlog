namespace WebBlog.Application
{
    public class GetCommentsRequest
    {   
        public Guid PostId { get; set;}
        public CCommentFilter CommentFilter { get; set; }
        public TablePageParameter Parameter { get; set; }

    }
}
