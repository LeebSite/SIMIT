using FluentValidation;
using Microsoft.AspNetCore.Http;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Constants;

namespace Pertamina.SIMIT.Shared.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;
public class CreateMahasiswaAttachmentRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public Guid MahasiswaId { get; set; }

    [OpenApiContentType(ContentTypesFor.MahasiswaAttachmentFile.Value)]
    public IFormFile File { get; set; } = default!;
}

public class CreateMahasiswaAttachmentRequestValidator : AbstractValidator<CreateMahasiswaAttachmentRequest>
{
    //private readonly long _maximumFileSize;

    public CreateMahasiswaAttachmentRequestValidator()
    {
        //_maximumFileSize = mahasiswaAttachmentOptions.Value.MaximumFileSizeInBytes;

        RuleFor(v => v.MahasiswaId)
            .NotEmpty();

        RuleFor(v => v.File.Length);

        RuleFor(v => v.File.ContentType)
            .Must(HaveSupportedContentType)
            .WithMessage($"{DisplayTextFor.Attachment} file extension is not supported. Please {CommonDisplayTextFor.Upload.ToLower()} supported file.");
    }
    private bool HaveSupportedContentType(string contentType)
    {
        return ContentTypesFor.MahasiswaAttachmentFile.List.Contains(contentType);
    }

}
