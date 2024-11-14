namespace Pertamina.SIMIT.Application.Services.Storage;

public interface IStorageService
{
    Task<string> CreateAsync(byte[] data);
    Task<byte[]> ReadAsync(string storedFileName);
    Task<string> ReadAsBase64Async(string storedFileName);
    Task<bool> DeleteAsync(string storedFileName);
    Task<string> UpdateAsync(string storedFileName, byte[] newData);
}
