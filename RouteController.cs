using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace hello_api;

[Route("api/[controller]/")]
public class MyController : Controller
{
    private readonly IBackgroundTaskQueue  _longLivingWorkQueue;
    private readonly ILogger<MyController> _logger;
    private readonly Func<WorkItem> _workItemFactory;

    public MyController(IBackgroundTaskQueue longLivingWorkQueue, ILogger<MyController> logger, Func<WorkItem> workItemFactory)
    {
        _longLivingWorkQueue = longLivingWorkQueue;
        _logger = logger;
        _workItemFactory = workItemFactory;
    }

    [HttpPut]
    [Route("UpdateRoute")]
    public async Task<IActionResult> UpdateRoute([FromBody]int _)
    {
        var workItem = _workItemFactory();
        _logger.LogInformation("Enqueue {Id} should take {secs}s", workItem.Id, workItem.Delay);
        await _longLivingWorkQueue.EnqueueAsync(workItem.CreateLongRunningTask());
        return Ok();
    }
}
