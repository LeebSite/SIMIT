using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Laporans.Constants;
public class DisplayTextFor
{
    public const string Laporan = nameof(Laporan);
    public const string Laporans = nameof(Laporans);
    public const string Nama = nameof(Nama);
    public const string Kampus = nameof(Kampus);

    public static readonly string ListLaporan = nameof(ListLaporan).SplitWords();
    public static readonly string RekapDokumenMahasiswa = nameof(RekapDokumenMahasiswa).SplitWords();
    public static readonly string DokumenMahasiswa = nameof(DokumenMahasiswa).SplitWords();
    public static readonly string CariNamaMahasiswa = nameof(CariNamaMahasiswa).SplitWords();
    public static readonly string StatusLaporan = nameof(StatusLaporan).SplitWords();
    public static readonly string StatusProject = nameof(StatusProject).SplitWords();
}
