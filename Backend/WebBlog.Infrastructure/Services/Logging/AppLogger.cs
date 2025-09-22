using Serilog;
using WebBlog.Application.Interfaces;

namespace WebBlog.Infrastructure.Services
{
    public class AppLogger : IAppLogger
    {
        private readonly ILogger _logger;
        public AppLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Info(string message)
        {
            _logger.Information(message);
        }

        public void Warning(string message)
        {
            _logger.Warning(message);
        }
    }
}
