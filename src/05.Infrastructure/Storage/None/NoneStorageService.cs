using Microsoft.Extensions.Logging;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Storage.None;

public class NoneStorageService : IStorageService
{
    private readonly ILogger<NoneStorageService> _logger;

    public NoneStorageService(ILogger<NoneStorageService> logger)
    {
        _logger = logger;
    }

    private void LogWarning()
    {
        _logger.LogWarning("{ServiceName} is set to None.", $"{nameof(Storage)} {CommonDisplayTextFor.Service}");
    }

    public Task<string> CreateAsync(byte[] data)
    {
        LogWarning();

        return Task.FromResult(string.Empty);
    }

    public Task<bool> DeleteAsync(string storageFileId)
    {
        LogWarning();

        return Task.FromResult(false);
    }

    public Task<string> ReadAsBase64Async(string storageFileId)
    {
        LogWarning();

        return Task.FromResult(string.Empty);
    }

    public Task<byte[]> ReadAsync(string storageFileId)
    {
        LogWarning();

        return Task.FromResult(Array.Empty<byte>());
    }

    public Task<string> UpdateAsync(string storageFileId, byte[] newData)
    {
        LogWarning();

        return Task.FromResult(string.Empty);
    }
}
