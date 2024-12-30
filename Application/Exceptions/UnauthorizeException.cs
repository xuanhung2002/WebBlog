namespace WebBlog.Application.Exceptions
{
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException() : base()
        {            
        }
        public UnauthorizeException(string message) : base(message)
        {           
        }
    }
}
