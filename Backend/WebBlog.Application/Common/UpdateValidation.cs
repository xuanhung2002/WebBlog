using WebBlog.Domain.Entities;

namespace WebBlog.Application
{
    public static class UpdateValidation
    {
        public static void ValidateUpdate<T>(T entity) where T : EntityAuditBase
        {
            if (entity.CreatedBy != RuntimeContext.CurrentUser.Id)
            {
                throw new UnauthorizedAccessException("You are not allowed to edit this entity");
            }
        }
    }
}
