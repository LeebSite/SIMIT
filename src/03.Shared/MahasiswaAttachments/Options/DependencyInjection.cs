using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pertamina.SIMIT.Shared.MahasiswaAttachments.Options;
public static class DependencyInjection
{
    public static IServiceCollection AddTicketAttachmentOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MahasiswaAttachmentOptions>(configuration.GetSection(MahasiswaAttachmentOptions.SectionKey));

        return services;
    }
}
