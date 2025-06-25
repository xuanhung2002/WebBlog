namespace WebBlog.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetById(Guid id);
        Task<List<CategoryDto>> GetAll();
    }
}
