namespace hello_api
{
    public class WorkItem
    {
        private static readonly Random Rnd = new();
        private static int _idGen = 0;
        public int Id { get; init; }

        public int Delay { get; set; } = Rnd.Next(10, 60);

        public WorkItem()
        {
            Id = ++_idGen;
        }

        public async Task<WorkItem> CreateLongRunningTask()
        {
            await Task.Delay(Delay * 1000);
            return this;
        }

        public string Summary => $"Task:{Id} with a delay of {Delay}s";
    }
}
