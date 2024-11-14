namespace Pertamina.SIMIT.Infrastructure.Persistence;

public class PersistenceOptions
{
    public const string SectionKey = nameof(Persistence);

    public string Provider { get; set; } = PersistenceProvider.None;
}

public static class PersistenceProvider
{
    public const string None = nameof(None);
    public const string SqlServer = nameof(SqlServer);
    public const string MySql = nameof(MySql);
}
