namespace TicketProcessor.Application.Configuration
{
    public class WorkerProcessingOptions
    {
        public int MaxConcurrency { get; set; } = 1;
        public int BackpressureLagThreshold { get; set; } = 100;
    }
}