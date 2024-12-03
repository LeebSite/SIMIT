
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.LogbookAttachments.Constants;
using Pertamina.SIMIT.Shared.Logbooks.Constants;

namespace Pertamina.SIMIT.Shared.Logbooks.Commands.CreateLogbook;
public class CreateLogbookRequest
{

    [OpenApiContentType(ContentTypes.TextPlain)]
    public DateTime? LogbookDate { get; set; } = DateTime.Now;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Aktifitas { get; set; } = default!;

    //[OpenApiContentType(ContentTypes.TextPlain)]
    //public Guid MahasiswaId { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string? MahasiswaNim { get; set; } = default!;

    [OpenApiContentType(ContentTypesFor.LogbookAttachmentFile.Value)]
    public IFormFile File { get; set; } = default!;

}

public class CreateLogbookRequestValidator : AbstractValidator<CreateLogbookRequest>
{
    public CreateLogbookRequestValidator()
    {
        RuleFor(v => v.Aktifitas)
          .NotEmpty()
          .MaximumLength(MaximumLengthFor.Aktifitas);

        RuleFor(v => v.MahasiswaNim)
         .NotEmpty();

        RuleFor(v => v.File)
            .NotEmpty();
    }
}

