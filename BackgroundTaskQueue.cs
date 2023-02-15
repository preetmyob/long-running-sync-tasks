using System.Diagnostics;
using System.Threading.Channels;

namespace hello_api;

[DebuggerDisplay("BackgroundTaskQueue ({Count})")] 
public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Task<WorkItem>> _channel = Channel.CreateUnbounded<Task<WorkItem>>();
    public async Task EnqueueAsync(Task<WorkItem> t)
    {
        await _channel.Writer.WriteAsync(t);
    }

    public async Task<Task<WorkItem>> DequeueAsync()
    {
        while(await _channel.Reader.WaitToReadAsync())
        {
            if (_channel.Reader.TryRead(out Task<WorkItem>? workItemTask))
            {
                return workItemTask;
            }
        }

        return Task<WorkItem>.FromResult(WorkItem.Empty);
    }

    public int Count => _channel.Reader.Count;
}
