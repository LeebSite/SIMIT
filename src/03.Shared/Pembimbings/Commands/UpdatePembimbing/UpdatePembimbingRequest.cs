using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Pembimbings.Constants;

namespace Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbing;
public class UpdatePembimbingRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public Guid PembimbingId { get; set; }

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Nama { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Nip { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Jabatan { get; set; } = default!;
}

public class UpdatePembimbingRequestValidator : AbstractValidator<UpdatePembimbingRequest>
{
    public UpdatePembimbingRequestValidator()
    {
        RuleFor(v => v.PembimbingId)
            .NotEmpty();

        RuleFor(v => v.Nama)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.Nama);

        RuleFor(v => v.Nip)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.Nip);

        RuleFor(v => v.Jabatan)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.Jabatan);
    }
}
