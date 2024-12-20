
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

    [OpenApiContentType(ContentTypes.TextPlain)]
    public Guid MahasiswaId { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string? MahasiswaNim { get; set; } = default!;

    //[OpenApiContentType(ContentTypesFor.LogbookAttachmentFile.Value)]
    //public IFormFile File { get; set; } = default!;
    [OpenApiContentType(ContentTypes.TextPlain)]
    public double Latitude { get; set; }

    [OpenApiContentType(ContentTypes.TextPlain)]
    public double Longitude { get; set; }

    [OpenApiContentType(ContentTypes.TextPlain)]
    public double Accuracy { get; set; }

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

        //RuleFor(v => v.File)
        //    .NotEmpty();

        RuleFor(v => v.Latitude)
           //.NotEmpty()
           .InclusiveBetween(-90, 90)
           .WithMessage("Latitude must be between -90 and 90 degrees.");

        RuleFor(v => v.Longitude)
            //.NotEmpty()
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180 degrees.");

        RuleFor(v => v.Accuracy);
        RuleFor(v => v.File)
            .NotEmpty();
    }
}

