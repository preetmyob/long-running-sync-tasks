using System.Diagnostics;
using System.Threading.Channels;

namespace hello_api;

[DebuggerDisplay("BackgroundTaskQueue ({Count})")] 
public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Task> _channel = Channel.CreateUnbounded<Task>();

    public async Task EnqueueAsync(Task t)
    {
        await _channel.Writer.WriteAsync(t);
    }

    public async Task<Task> DequeueAsync()
    {
        while(await _channel.Reader.WaitToReadAsync())
        {
            if (_channel.Reader.TryRead(out Task? workItemTask))
            {
                return workItemTask;
            }
        }
        
        return Task.FromResult(new WorkItem());
    }

    public int Count => _channel.Reader.Count;
}
