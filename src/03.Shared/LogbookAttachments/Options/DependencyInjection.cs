using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pertamina.SIMIT.Shared.LogbookAttachments.Options;
public static class DependencyInjection
{
    public static IServiceCollection AddTicketAttachmentOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LogbookAttachmentOptions>(configuration.GetSection(LogbookAttachmentOptions.SectionKey));

        return services;
    }
}
