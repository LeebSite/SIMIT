
using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;
public class GetMahasiswaResponse : Response
{
    public Guid Id { get; set; }
    public string Nama { get; set; }
    public string Nim { get; set; } = default!;
    public string Kampus { get; set; } = default!;
    public DateTime MulaiMagang { get; set; } = default!;
    public DateTime SelesaiMagang { get; set; } = default!;
    public string Bagian { get; set; } = default!;
    public Guid PembimbingId { get; set; }

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
