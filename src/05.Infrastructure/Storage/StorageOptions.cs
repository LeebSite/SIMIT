namespace Pertamina.SIMIT.Infrastructure.Storage;

public class StorageOptions
{
    public const string SectionKey = nameof(Storage);

    public string Provider { get; set; } = StorageProvider.None;
}

public static class StorageProvider
{
    public const string None = nameof(None);
    public const string LocalFolder = nameof(LocalFolder);
    public const string AzureBlob = nameof(AzureBlob);
}
