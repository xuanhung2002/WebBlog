namespace WebBlog.Application.Interfaces
{
    public interface IAppLogger<T>
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Critical(string message);
        void Debug(string message);
    }
}
