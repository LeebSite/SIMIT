using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Extensions;
using Pertamina.SIMIT.Shared.LogbookAttachments.Constants;
using Pertamina.SIMIT.Shared.LogbookAttachments.Options;

namespace Pertamina.SIMIT.Shared.LogbookAttachments.CreateLogbookAttachments;
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

    public CreateLogbookAttachmentRequestValidator(IOptions<LogbookAttachmentOptions> logbookAttachmentOptions)
    {
        _maximumFileSize = logbookAttachmentOptions.Value.MaximumFileSizeInBytes;

        RuleFor(v => v.LogbookId)
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
        return ContentTypesFor.LogbookAttachmentFile.List.Contains(contentType);
    }
}
