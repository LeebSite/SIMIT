using Pertamina.SIMIT.Application.Services.Ecm.Models.UploadContent;

namespace Pertamina.SIMIT.Application.Services.Ecm;

public interface IEcmService
{
    Task<UploadContentResponse> UploadContentAsync(UploadContentRequest request, CancellationToken cancellationToken);
}
