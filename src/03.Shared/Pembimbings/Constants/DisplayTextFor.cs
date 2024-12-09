using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Pembimbings.Constants;
public class DisplayTextFor
{
    public static readonly string RekapDataPembimbing = nameof(RekapDataPembimbing).SplitWords();
    public static readonly string DataPembimbing = nameof(DataPembimbing).SplitWords();
    public const string Pembimbings = nameof(Pembimbings);
    public const string Pembimbing = nameof(Pembimbing);

    public const string Nama = nameof(Nama);
    public const string Nip = nameof(Nip);
    public const string Jabatan = nameof(Jabatan);
}
