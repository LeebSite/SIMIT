namespace Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooks;
public class GetLogbooksLogbook
{
    public string Id { get; set; }
    public Guid LogbookId { get; set; }
    public DateTime LogbookDate { get; set; } = default!;
    public string Aktifitas { get; set; } = default!;

    public Guid MahasiswaId { get; set; }

    public string? MahasiswaNama { get; set; }
    public string? MahasiswaNim { get; set; }

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
}
