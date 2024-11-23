using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Extensions;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Constants;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Options;

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
    private readonly long _maximumFileSize;

    public CreateMahasiswaAttachmentRequestValidator(IOptions<MahasiswaAttachmentOptions> mahasiswaAttachmentOptions)
    {
        _maximumFileSize = mahasiswaAttachmentOptions.Value.MaximumFileSizeInBytes;

        RuleFor(v => v.MahasiswaId)
            .NotEmpty();

        RuleFor(v => v.File.Length)
           .ExclusiveBetween(0L, _maximumFileSize)
           .WithMessage($"{DisplayTextFor.Attachment} file size should be greater than 0 KB and less than {_maximumFileSize.ToReadableFileSize()}.");

        RuleFor(v => v.File.ContentType)
            .Must(HaveSupportedContentType)
            .WithMessage($"{DisplayTextFor.Attachment} file extension is not supported. Please {CommonDisplayTextFor.Upload.ToLower()} supported file.");
    }
    private bool HaveSupportedContentType(string contentType)
    {
        return ContentTypesFor.MahasiswaAttachmentFile.List.Contains(contentType);
    }

}
