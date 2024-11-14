namespace Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbings;
public class GetPembimbingsPembimbing
{
    public Guid PembimbingId { get; set; }
    public string Nama { get; set; } = default!;
    public string Nip { get; set; } = default!;
    public string Jabatan { get; set; } = default!;
}
