namespace TicketProcessor.Application.Configuration
{
    public class SqlOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string Schema { get; set; } = "dbo";
    }
}