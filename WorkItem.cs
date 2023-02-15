namespace hello_api
{
    public class WorkItem
    {
        private ILogger<WorkItem>? _Logger;

        public WorkItem(Guid? id = default, ILogger<WorkItem>? logger = default)
        {
            _Logger = logger;
            Id = id ?? Guid.NewGuid();
        }

        public Guid Id { get; init; }
        public static WorkItem Empty { get; } = new WorkItem(Guid.Empty);

        public Task<WorkItem> CreateLongRunningTask()
        {
            var longRunningTask = new Task<WorkItem>( async () =>
                {
                    _Logger.LogInformation("Starting:{Id}", this.Id);
                    await Task.Delay(1000);
                    _Logger.LogInformation("Finishing:{Id}", this.Id);
                    return  this;
                }
            );
            return longRunningTask;
        }
    }
}
