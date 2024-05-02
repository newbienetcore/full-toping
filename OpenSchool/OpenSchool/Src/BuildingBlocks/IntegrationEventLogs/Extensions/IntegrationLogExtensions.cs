namespace IntegrationEventLogs;

public static class IntegrationLogExtensions
{
    public static void UseIntegrationEventLogs(this ModelBuilder builder)
    {
        builder.Entity<IntegrationEventLogEntry>(builder =>
        {
            builder.ToTable("integration_event_log");

            builder.HasKey(e => e.EventId);
        });
    }
}