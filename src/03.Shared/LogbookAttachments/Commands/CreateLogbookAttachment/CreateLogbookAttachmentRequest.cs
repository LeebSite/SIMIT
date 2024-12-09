using FluentValidation;
using Microsoft.AspNetCore.Http;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.LogbookAttachments.Constants;

namespace Pertamina.SIMIT.Shared.LogbookAttachments.Commands.CreateLogbookAttachment;
public class CreateLogbookAttachmentRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public Guid LogbookId { get; set; }

    [OpenApiContentType(ContentTypesFor.LogbookAttachmentFile.Value)]
    public IFormFile File { get; set; } = default!;
}

public class CreateLogbookAttachmentRequestValidator : AbstractValidator<CreateLogbookAttachmentRequest>
{
    private readonly long _maximumFileSize;

    public CreateLogbookAttachmentRequestValidator()
    {
        //_maximumFileSize = logbookAttachmentOptions.Value.MaximumFileSizeInBytes;

        RuleFor(v => v.LogbookId)
            .NotEmpty();

        //RuleFor(v => v.File.Length);

        //RuleFor(v => v.File.ContentType)
        //    .Must(HaveSupportedContentType)
        //    .WithMessage($"{DisplayTextFor.Attachment} file extension is not supported. Please {CommonDisplayTextFor.Upload.ToLower()} supported file.");
    }

    private bool HaveSupportedContentType(string contentType)
    {
        return ContentTypesFor.LogbookAttachmentFile.List.Contains(contentType);
    }
}
