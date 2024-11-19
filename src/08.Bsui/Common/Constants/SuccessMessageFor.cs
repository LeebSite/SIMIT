
namespace Pertamina.SIMIT.Bsui.Common.Constants;

public static class SuccessMessageFor
{
    public static string Action(string entityType, string actionName, bool isPlural = false)
    {
        return $"{entityType} {(isPlural ? "have" : "has")} been successfully {actionName.ToLower()}.";
    }

    public static string Action(string entityType, string entityFieldValue, string actionName, string deleted)
    {
        return $"{entityType} {entityFieldValue} has been successfully {actionName.ToLower()}.";
    }

    internal static string Action(string mahasiswa, string nama, string deleted)
    {
        throw new NotImplementedException();
    }
}
