using System.Collections.Concurrent;

namespace hello_api;

public class TaskProcessorService : BackgroundService
{
    private readonly IBackgroundTaskQueue _queue;
    private readonly ILogger<MyController> _logger;
    private List<Task<WorkItem>> workItemsTasksInProgress = new();
    private List<WorkItem> workItemsDone = new();

    
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
            // Grab a list of the items in progress but just as generic Task not as Task<WorkItem> 
            var itemsInProgress = workItemsTasksInProgress.Cast<Task>();
            
            // now add add the dequeuing task to this list
            var dequeueATaskOfWorkitemTask = _queue.DequeueAsync();
            var tasksToWaitFor = itemsInProgress.Append(dequeueATaskOfWorkitemTask).ToArray();
            
            // wait for for either a dequeue completion event (new item arrived) or 
            // one of te workItem tasks finishing.
            var someFinishedTask = await Task.WhenAny(tasksToWaitFor);

            
            /* TODO how to handle cancellations */
            
            // if it's a workItem then remove it from to be done and move to Done for later use
            if (someFinishedTask == dequeueATaskOfWorkitemTask)
            {
                // Grab the task created by the CreateLongRunningTask as the result of the dequeue operation
                var workItemInProgress = dequeueATaskOfWorkitemTask.Result;
                workItemsTasksInProgress.Add(workItemInProgress);
            }
            else
            {
                // remove from the tasksInProgress
                var theFinishedWorkItemTask = (Task<WorkItem>)someFinishedTask;
                workItemsTasksInProgress.Remove(theFinishedWorkItemTask);
                
                // add the finished workItem to the done list
                var theFinishedWorkItem = theFinishedWorkItemTask.Result;
                workItemsDone.Add(theFinishedWorkItem);
                Console.WriteLine($"Summary from finished workItem {theFinishedWorkItem.Summary}");
            }
        } while (true);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Ending the background service");
        return base.StopAsync(cancellationToken);
    }
}
