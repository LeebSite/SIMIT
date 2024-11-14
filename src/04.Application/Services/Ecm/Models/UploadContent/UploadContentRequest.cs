namespace Pertamina.SIMIT.Application.Services.Ecm.Models.UploadContent;

public class UploadContentRequest
{
    public string CompanyCode { get; set; } = default!;
    public string DocumentCategoryName { get; set; } = default!;
    public int JrdpYear { get; set; }
    public string TopicCode { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string FileContentType { get; set; } = default!;
    public byte[] FileContent { get; set; } = Array.Empty<byte>();
}
