using System.Collections.Concurrent;

namespace hello_api;

public class TaskProcessorService : BackgroundService
{
    private readonly IBackgroundTaskQueue _queue;
    private readonly ILogger<MyController> _logger;
    List<Task> workItemsToDo = new();
    List<Task> workItemsDone = new();

    
    public TaskProcessorService(IBackgroundTaskQueue queue, ILogger<MyController> logger)
    {
        _queue = queue;
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting the background service");
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            var newItemTask = _queue.DequeueAsync();
            
            var tasksToWaitFor = workItemsToDo.Append(newItemTask).ToArray();
            // wait for one of list of jobs to complete
            var finishedTask = await Task.WhenAny(tasksToWaitFor);

            
            /* TODO how to handle cancellations */
            
            // if it's a workItem then remove it from to be done and move to Done for later use
            if (finishedTask != newItemTask)
            {
                if(workItemsToDo.Remove(finishedTask))
                {
                    workItemsDone.Add(finishedTask);
                }
            }
            else // if it's a new workItem then add it the to be done list
            {
                var workItemToDo = finishedTask;
                workItemsToDo.Add(workItemToDo);
            }

        } while (true);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Ending the background service");
        return base.StopAsync(cancellationToken);
    }
}