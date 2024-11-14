namespace Pertamina.SIMIT.Infrastructure.BackgroundJob.Hangfire.Storages.MySql;

public class MySqlOptions
{
    public static readonly string SectionKey = $"{nameof(BackgroundJob)}:{nameof(Hangfire)}:{nameof(HangfireBackgroundJobOptions.Storage)}:{nameof(MySql)}";

    public string ConnectionString { get; set; } = default!;
}
