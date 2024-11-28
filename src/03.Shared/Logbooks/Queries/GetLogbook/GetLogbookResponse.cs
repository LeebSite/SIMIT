using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbook;
public class GetLogbookResponse : Response
{
    public Guid Id { get; set; }
    public DateTime LogbookDate { get; set; } = default!;
    public string Aktifitas { get; set; } = default!;

    public Guid MahasiswaId { get; set; }
    public string? MahasiswaNim { get; set; }
    public string? MahasiswaNama { get; set; }

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
