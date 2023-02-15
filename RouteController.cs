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
        Guid id = Guid.NewGuid();
        _logger.LogInformation("New Job {Id}", id);
        await _longLivingWorkQueue.EnqueueAsync(CreateTheLongRunningTask(id));
        return Ok();
    }

    private Task CreateTheLongRunningTask(Guid id)
    {
        return new Task(async () =>
        {
            _logger.LogInformation("Starting:{Id}", id);
            await Task.Delay(1000);
            _logger.LogInformation("Finishing:{Id}", id);
        });
    }
}