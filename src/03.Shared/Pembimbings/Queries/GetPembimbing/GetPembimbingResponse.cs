using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbing;
public class GetPembimbingResponse : Response
{
    public Guid PembimbingId { get; set; }

    public string Nama { get; set; } = default!;
    public string Nip { get; set; } = default!;
    public string Jabatan { get; set; } = default!;

}
