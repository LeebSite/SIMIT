namespace Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;
public class GetMahasiswaMahasiswaAttachment
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = default!;
    public long FileSize { get; set; }
    public string Description { get; set; } = default!;

    public Guid MahasiswaId { get; set; }

    public IList<GetMahasiswaMahasiswaAttachment> Attachments { get; set; } = new List<GetMahasiswaMahasiswaAttachment>();

}
