namespace WebBlog.Infrastructure.Persistance
{
    internal static class TableNames
    {
        internal const string AppUsers = nameof(AppUsers);
        internal const string AppRoles = nameof(AppRoles);

        internal const string AppUserRoles = nameof(AppUserRoles);
        internal const string AppUserClaims = nameof(AppUserClaims);
        internal const string AppUserLogins = nameof(AppUserLogins);
        internal const string AppUserTokens = nameof(AppUserTokens);

        internal const string RefreshToken = nameof(RefreshToken);

        internal const string Post = nameof(Post);
        internal const string PostReaction = nameof(PostReaction);
        internal const string Comment = nameof(Comment);
        internal const string CommentReaction = nameof(CommentReaction);
        internal const string Category = nameof(Category);
        internal const string SubCategory = nameof(SubCategory);
        internal const string SearchHistory = nameof(SearchHistory);
        internal const string WatchingPost = nameof(WatchingPost);
    }
}
