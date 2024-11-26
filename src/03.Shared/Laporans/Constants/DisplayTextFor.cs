using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Laporans.Constants;
public class DisplayTextFor
{
    public const string Laporans = nameof(Laporans);
    public const string Laporan = nameof(Laporan);

    public const string Mahasiswa = nameof(Mahasiswa);
    public const string Deskripsi = nameof(Deskripsi);
    public const string Search = nameof(Search);
    public const string Nama = nameof(Nama);
    public const string Kampus = nameof(Kampus);
    public const string Nim = nameof(Nim);
    public const string Dokumen = nameof(Dokumen);
    public const string Simpan = nameof(Simpan);

    public static readonly string DokumenMahasiswa = nameof(DokumenMahasiswa).SplitWords();
    public static readonly string RekapDokumenMahasiswa = nameof(RekapDokumenMahasiswa).SplitWords();
    public static readonly string TambahLaporanDanProyekMagang = nameof(TambahLaporanDanProyekMagang).SplitWords();
}
