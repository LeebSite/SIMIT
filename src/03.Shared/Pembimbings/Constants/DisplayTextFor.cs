using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Pembimbings.Constants;
public class DisplayTextFor
{
    public const string Pembimbings = nameof(Pembimbings);
    public const string Pembimbing = nameof(Pembimbing);
    public static readonly string ListPembimbing = nameof(ListPembimbing).SplitWords();

    public const string Nama = nameof(Nama);
    public static readonly string Nip = nameof(Nip).ToUpper();
    public const string Jabatan = nameof(Jabatan);
}
