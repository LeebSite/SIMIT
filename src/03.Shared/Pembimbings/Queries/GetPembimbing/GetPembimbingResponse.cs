using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbing;
public class GetPembimbingResponse : Response
{
    public Guid Id { get; set; }

    public string Nama { get; set; } = default!;
    public string Nip { get; set; } = default!;
    public string Jabatan { get; set; } = default!;
    public string Email { get; set; } = default!;

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;

}
