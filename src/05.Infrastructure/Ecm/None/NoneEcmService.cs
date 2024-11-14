using Microsoft.Extensions.Logging;
using Pertamina.SIMIT.Application.Services.Ecm;
using Pertamina.SIMIT.Application.Services.Ecm.Models.UploadContent;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Ecm.None;

public class NoneEcmService : IEcmService
{
    private readonly ILogger<NoneEcmService> _logger;

    public NoneEcmService(ILogger<NoneEcmService> logger)
    {
        _logger = logger;
    }

    private void LogWarning()
    {
        _logger.LogWarning("{ServiceName} is set to None.", $"{nameof(Ecm).ToUpper()} {CommonDisplayTextFor.Service}");
    }

    public Task<UploadContentResponse> UploadContentAsync(UploadContentRequest uploadContentModel, CancellationToken cancellationToken)
    {
        LogWarning();

        return Task.FromResult(new UploadContentResponse
        {
            ContentId = null
        });
    }
}
