using Microsoft.Extensions.Hosting;

namespace WebBlog.Infrastructure.Workers
{
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;

        public BackgroundWorkerService(IBackgroundTaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation(
            //$"Queued Hosted Service is running.{Environment.NewLine}" +
            //$"{Environment.NewLine}Tap W to add a work item to the " +
            //$"background queue.{Environment.NewLine}");

            await BackgroundProcessing(stoppingToken);
        }
        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem =
                    await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex,
                    //    "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
