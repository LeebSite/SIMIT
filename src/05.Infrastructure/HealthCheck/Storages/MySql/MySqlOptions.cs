namespace Pertamina.SIMIT.Infrastructure.HealthCheck.Storages.MySql;

public class MySqlOptions
{
    public static readonly string SectionKey = $"{nameof(HealthCheck)}:{nameof(HealthCheckOptions.UI)}:{nameof(HealthCheckOptions.UI.Storage)}:{nameof(MySql)}";

    public string ConnectionString { get; set; } = default!;
}
