namespace Pertamina.SIMIT.Infrastructure.Storage.AzureBlob;

public class AzureBlobStorageOptions
{
    public static readonly string SectionKey = $"{nameof(Storage)}:{nameof(AzureBlob)}";

    public string ConnectionString { get; set; } = default!;
    public string ContainerName { get; set; } = default!;
}
