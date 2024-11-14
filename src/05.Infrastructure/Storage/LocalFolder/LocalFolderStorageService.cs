using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Services.Storage;

namespace Pertamina.SIMIT.Infrastructure.Storage.LocalFolder;

public class LocalFolderStorageService : IStorageService
{
    private readonly string _folderPath;
    private readonly ILogger<LocalFolderStorageService> _logger;

    public LocalFolderStorageService(IOptions<LocalFolderStorageOptions> localFolderStorageOptions, ILogger<LocalFolderStorageService> logger)
    {
        _folderPath = localFolderStorageOptions.Value.FolderPath;
        _logger = logger;
    }

    public async Task<string> CreateAsync(byte[] data)
    {
        var storageFileId = $"{Guid.NewGuid()}{Guid.NewGuid()}";

        return await CreateAsync(data, storageFileId);
    }

    private async Task<string> CreateAsync(byte[] data, string storageFileId)
    {
        try
        {
            var filePath = Path.Combine(_folderPath, storageFileId);

            using var fileStream = File.Create(filePath);
            await fileStream.WriteAsync(data.AsMemory(0, data.Length));

            return storageFileId;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error in executing method {MethodName} with Storage File Id {StorageFileId}", nameof(CreateAsync), storageFileId);

            throw;
        }
    }

    public Task<byte[]> ReadAsync(string storageFileId)
    {
        try
        {
            return File.ReadAllBytesAsync(Path.Combine(_folderPath, storageFileId));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error in executing method {MethodName} with Storage File Id {StorageFileId}", nameof(ReadAsync), storageFileId);

            throw;
        }
    }

    public async Task<string> ReadAsBase64Async(string storageFileId)
    {
        var fileBytes = await ReadAsync(storageFileId);
        var result = Convert.ToBase64String(fileBytes);

        return result;
    }

    public Task<bool> DeleteAsync(string storageFileId)
    {
        try
        {
            var filePath = Path.Combine(_folderPath, storageFileId);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error in executing method {MethodName} with Storage File Id {StorageFileId}", nameof(DeleteAsync), storageFileId);

            throw;
        }
    }

    public async Task<string> UpdateAsync(string storageFileId, byte[] newData)
    {
        try
        {
            if (await DeleteAsync(storageFileId))
            {
                var result = await CreateAsync(newData);

                return result;
            }
            else
            {
                var exception = new Exception($"Unable to delete file {storageFileId}");

                _logger.LogError(exception, "Error in executing method {MethodName} with Storage File Id {StorageFileId}", nameof(UpdateAsync), storageFileId);

                return string.Empty;
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error in executing method {MethodName} with data location {Location}", nameof(UpdateAsync), storageFileId);

            throw;
        }
    }
}
