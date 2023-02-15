namespace hello_api;

public interface IBackgroundTaskQueue
{
    Task EnqueueAsync(Task t);
    Task<Task> DequeueAsync();
    int Count { get; }
}
