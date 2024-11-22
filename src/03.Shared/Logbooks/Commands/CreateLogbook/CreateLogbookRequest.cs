
using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Logbooks.Constants;

namespace Pertamina.SIMIT.Shared.Logbooks.Commands.CreateLogbook;
public class CreateLogbookRequest
{
    //[OpenApiContentType(ContentTypes.TextPlain)]
    //public string MahasiswaNim { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public DateTime? LogbookDate { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Aktifitas { get; set; } = default!;
    public Guid MahasiswaId { get; set; } = default!;

}

public class CreateLogbookRequestValidator : AbstractValidator<CreateLogbookRequest>
{
    public CreateLogbookRequestValidator()
    {
        RuleFor(v => v.Aktifitas)
          .NotEmpty()
          .MaximumLength(MaximumLengthFor.Aktifitas);

        //RuleFor(v => v.MahasiswaNim)
        // .NotEmpty();
    }
}
