using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace hello_api;

[Route("api/[controller]/")]
public class MyController : Controller
{
    private readonly IBackgroundTaskQueue  _longLivingWorkQueue;
    private readonly ILogger<MyController> _logger;

    public MyController(IBackgroundTaskQueue longLivingWorkQueue, ILogger<MyController> logger)
    {
        _longLivingWorkQueue = longLivingWorkQueue;
        _logger = logger;
    }

    [HttpPut]
    [Route("UpdateRoute")]
    public async Task<IActionResult> UpdateRoute([FromBody]int _)
    {
        var workItem = new WorkItem();
        // _logger.LogInformation("Enqueue {Id}", workItem.Id);
        await _longLivingWorkQueue.EnqueueAsync(workItem.CreateLongRunningTask());
        return Ok();
    }
}
