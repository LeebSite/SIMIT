using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Pembimbings.Constants;
public class DisplayTextFor
{
    public static readonly string RekapDataPembimbing = nameof(RekapDataPembimbing).SplitWords();
    public static readonly string DataPembimbing = nameof(DataPembimbing).SplitWords();
    public const string Pembimbings = nameof(Pembimbings);
    public const string Pembimbing = nameof(Pembimbing);
    public static readonly string ListPembimbing = nameof(ListPembimbing).SplitWords();
    public static readonly string DetailDataPembimbing = nameof(DetailDataPembimbing).SplitWords();

    public const string Nama = nameof(Nama);
    public static readonly string Nip = nameof(Nip).ToUpper();
    public const string Jabatan = nameof(Jabatan);
}
