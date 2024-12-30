﻿using System.Threading.Channels;

namespace WebBlog.Infrastructure.Workers
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem);

        ValueTask<Func<CancellationToken, Task>> DequeueAsync(
            CancellationToken cancellationToken);
    }
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue;

        //public BackgroundTaskQueue(int capacity)
        //{
        //    // Capacity should be set based on the expected application load and
        //    // number of concurrent threads accessing the queue.            
        //    // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
        //    // which completes only when space became available. This leads to backpressure,
        //    // in case too many publishers/calls start accumulating.
        //    var options = new BoundedChannelOptions(capacity)
        //    {
        //        FullMode = BoundedChannelFullMode.Wait
        //    };
        //    _queue = Channel.CreateBounded<Func<CancellationToken, Task>>(options);
        //}
        public BackgroundTaskQueue()
        {
            _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();
        }

        // add task to queue
        public async ValueTask QueueBackgroundWorkItemAsync(
            Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            await _queue.Writer.WriteAsync(workItem);
        }

        // get task and run
        public async ValueTask<Func<CancellationToken, Task>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);
            if (workItem == null)
            {
                await Task.Delay(100, cancellationToken);
            }
            return workItem;
        }
    }
}
