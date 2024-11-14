namespace Pertamina.SIMIT.Domain.Interfaces;

public interface IHasFile
{
    string FileName { get; set; }
    string FileContentType { get; set; }
    long FileSize { get; set; }
    string StorageFileId { get; set; }
}
