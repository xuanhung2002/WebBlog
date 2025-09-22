namespace WebBlog.Application.Interfaces
{
    public interface IAppLogger
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Debug(string message);
    }
}
