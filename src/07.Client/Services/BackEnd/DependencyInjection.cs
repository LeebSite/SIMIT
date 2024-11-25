using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pertamina.SIMIT.Client.Services.BackEnd;

public static class DependencyInjection
{
    public static IServiceCollection AddBackEndService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BackEndOptions>(configuration.GetSection(BackEndOptions.SectionKey));

        #region Essential Services
        services.AddTransient<AuditService>();
        #endregion Essential Services

        #region Business Services
        services.AddTransient<MahasiswaService>();
        services.AddTransient<MahasiswaAttachmentService>();
        services.AddTransient<PembimbingService>();
        services.AddTransient<LaporanService>();
        #endregion Business Services

        return services;
    }
}
