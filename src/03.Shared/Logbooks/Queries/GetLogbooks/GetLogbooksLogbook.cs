using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooks;
public class GetLogbooksLogbook : FileResponse
{
    public string Id { get; set; }
    public Guid LogbookId { get; set; }
    public DateTime LogbookDate { get; set; } = default!;
    public string Aktifitas { get; set; } = default!;

    public Guid MahasiswaId { get; set; }

    public Guid LogbookAttachmentId { get; set; }
    public bool StatusPagi { get; set; }
    public bool StatusSiang { get; set; }

    // Contoh properti tambahan di GetLogbooksLogbook
    public string? ImageBase64Url => Content != null
    ? $"data:{ContentType ?? "image/png"};base64,{Convert.ToBase64String(Content)}"
    : null;

    public string? MahasiswaNama { get; set; }
    public string? MahasiswaNim { get; set; }

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
}
