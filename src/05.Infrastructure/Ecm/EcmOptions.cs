namespace Pertamina.SIMIT.Infrastructure.Ecm;

public class EcmOptions
{
    public const string SectionKey = nameof(Ecm);

    public string Provider { get; set; } = EcmProvider.None;
}

public static class EcmProvider
{
    public const string None = nameof(None);
    public const string Idms = nameof(Idms);
}
