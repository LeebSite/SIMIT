
using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Mahasiswas.Constants;
public class DisplayTextFor
{
    public const string Mahasiswas = nameof(Mahasiswas);
    public const string Mahasiswa = nameof(Mahasiswa);

    public static readonly string RekapDataMahasiswa = nameof(RekapDataMahasiswa).SplitWords();
    public static readonly string DetailDataMahasiwa = nameof(DetailDataMahasiwa).SplitWords();
    public static readonly string DataMahasiswa = nameof(DataMahasiswa).SplitWords();

    public const string Nama = nameof(Nama);
    public const string Deskripsi = nameof(Deskripsi);
    public const string Nim = nameof(Nim);
    public const string Kampus = nameof(Kampus);
    public static readonly string MulaiMagang = nameof(MulaiMagang).SplitWords();
    public static readonly string SelesaiMagang = nameof(SelesaiMagang).SplitWords();
    public const string Bagian = nameof(Bagian);
    public const string Pembimbing = nameof(Pembimbing);
    public const string PembimbingId = nameof(PembimbingId);
    public static readonly string StatusDokumen = nameof(StatusDokumen).SplitWords();
    public static readonly string DeskripsiDokumen = nameof(DeskripsiDokumen).SplitWords();
    public static readonly string DetailLogbook = nameof(DetailLogbook).SplitWords();
}
