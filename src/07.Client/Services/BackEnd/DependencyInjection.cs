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
        services.AddTransient<PembimbingService>();
<<<<<<< HEAD
        services.AddTransient<LogbookService>();
=======
        services.AddTransient<LaporanService>();
>>>>>>> 19f0b79213536f55caf243451d436d533452b8ea
        #endregion Business Services

        return services;
    }
}
