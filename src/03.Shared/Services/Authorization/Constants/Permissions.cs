namespace Pertamina.SIMIT.Shared.Services.Authorization.Constants;

public static class Permissions
{
    #region Essential Permissions
    public const string SolTem_Audit_Index = "soltem.audit.index";
    public const string SolTem_Audit_View = "soltem.audit.view";
    public const string SolTem_HealthCheck_View = "soltem.healthcheck.view";
    #endregion Essential Permissions

    #region Business Permissions
    #endregion Business Permissions

    public static readonly string[] All = new string[]
    {
        #region Essential Permissions
        SolTem_Audit_Index,
        SolTem_Audit_View,
        SolTem_HealthCheck_View,
        #endregion Essential Permissions

        #region Business Permissions
        #endregion Business Permissions
    };
}
