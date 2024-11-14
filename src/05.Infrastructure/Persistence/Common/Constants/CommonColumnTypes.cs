namespace Pertamina.SIMIT.Infrastructure.Persistence.Common.Constants;

public static class CommonColumnTypes
{
    public const string VarbinaryMax = "varbinary(max)";

    public static string Nvarchar(int length)
    {
        return $"nvarchar({length})";
    }
}
