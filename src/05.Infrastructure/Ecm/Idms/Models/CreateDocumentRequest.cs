using Microsoft.AspNetCore.Http;
using Pertamina.SIMIT.Application.Services.Ecm.Models.UploadContent;

namespace Pertamina.SIMIT.Infrastructure.Ecm.Idms.Models;

public class CreateDocumentRequest
{
    public Guid ClientApplicationId { get; set; }
    public string CompanyCode { get; set; } = default!;
    public string DocumentCategoryName { get; set; } = default!;
    public int JrdpYear { get; set; }
    public string TopicCode { get; set; } = default!;
    public IFormFile? DocumentFile { get; set; }

    public static CreateDocumentRequest ToCreateDocumentRequest(UploadContentRequest documentModel, Guid clientApplicationId)
    {
        return new CreateDocumentRequest
        {
            ClientApplicationId = clientApplicationId,
            CompanyCode = documentModel.CompanyCode,
            DocumentCategoryName = documentModel.DocumentCategoryName,
            JrdpYear = documentModel.JrdpYear,
            TopicCode = documentModel.TopicCode
        };
    }
}
