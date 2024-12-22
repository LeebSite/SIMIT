using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Logbooks.Constants;
public class DisplayTextFor
{
    public const string Logbooks = nameof(Logbooks);
    public const string Logbook = nameof(Logbook);

    public static readonly string LogbookHarianMahasiswa = nameof(LogbookHarianMahasiswa).SplitWords();
    public static readonly string NoBadgeMahasiswa = nameof(NoBadgeMahasiswa).SplitWords();
    public static readonly string StatusLogbookPagi = nameof(StatusLogbookPagi).SplitWords();
    public static readonly string StatusLogbookSiang = nameof(StatusLogbookSiang).SplitWords();
    public static readonly string NamaMahasiswa = nameof(NamaMahasiswa).SplitWords();
    public static readonly string RekapLogbookMahasiswa = nameof(RekapLogbookMahasiswa).SplitWords();
    public static readonly string StartKamera = nameof(StartKamera).SplitWords();
    public static readonly string AmbilFoto = nameof(AmbilFoto).SplitWords();
    public static readonly string SwitchKamera = nameof(SwitchKamera).SplitWords();

    public const string Tanggal = nameof(Tanggal);
    public const string Detail = nameof(Detail);
    public const string Aktifitas = nameof(Aktifitas);
    public const string Nim = nameof(Nim);
    public static readonly string MahasiswaNama = nameof(MahasiswaNama).SplitWords();
    public static readonly string MahasiswaNim = nameof(MahasiswaNim).SplitWords();

    public const string Mahasiswa = nameof(Mahasiswa);
    public const string MahasiswaId = nameof(MahasiswaId);

    public static readonly string ListLogbook = nameof(ListLogbook).SplitWords();

}

