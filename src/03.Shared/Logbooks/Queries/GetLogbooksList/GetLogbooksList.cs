namespace Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooksList;
public class GetLogbooksList
{
    public Guid Id { get; set; }
    public string Aktivitas { get; set; }

    public DateTime LogbookDate { get; set; } = default!;
}
