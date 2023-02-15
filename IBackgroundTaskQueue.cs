namespace hello_api;

public interface IBackgroundTaskQueue
{
    Task EnqueueAsync(Task<WorkItem> t);
    Task<Task<WorkItem>> DequeueAsync();
    int Count { get; }
}
