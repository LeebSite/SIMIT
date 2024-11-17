using Pertamina.SIMIT.Domain.Abstracts;

namespace Pertamina.SIMIT.Domain.Entities;
public class Pembimbing : Entity
{
    public string Nama { get; set; } = default!;
    public string Nip { get; set; } = default!;
    public string Jabatan { get; set; } = default!;

    public List<Mahasiswa> Mahasiswas { get; set; } = new List<Mahasiswa>();
}
