
using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;
public class GetMahasiswaResponse : Response
{
    public Guid Id { get; set; }
    public string Nama { get; set; } = default!;
    public string Nim { get; set; } = default!;
    public string Kampus { get; set; } = default!;
    public DateTime MulaiMagang { get; set; } = default!;
    public DateTime SelesaiMagang { get; set; } = default!;
    public string Bagian { get; set; } = default!;
    public Guid PembimbingId { get; set; }
    public string? PembimbingNama { get; set; }
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public Guid MahasiswaAttachmentId { get; set; }
    public Guid LaporanId { get; set; }
    public string? LaporanDeskripsi { get; set; }

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }

    //public IList<GetMahasiswaMahasiswaAttachment> Attachments { get; set; } = new List<GetMahasiswaMahasiswaAttachment>();

}
