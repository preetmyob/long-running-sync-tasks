namespace hello_api
{
    public class WorkItem
    {
        public Guid Id { get; init; }

        public WorkItem()
        {
            Id = Guid.NewGuid();
        }

        public async Task<WorkItem> CreateLongRunningTask()
        {
            Console.WriteLine("Starting:{0}", this.Id);
            await Task.Delay(1000);
            Console.WriteLine("Finishing:{0}", this.Id);
            return new WorkItem();
        }
    }
}
