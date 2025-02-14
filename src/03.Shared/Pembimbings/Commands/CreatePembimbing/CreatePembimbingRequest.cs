using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Pembimbings.Constants;

namespace Pertamina.SIMIT.Shared.Pembimbings.Commands.CreatePembimbing;
public class CreatePembimbingRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Nama { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Nip { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Jabatan { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Email { get; set; } = default!;
}

public class CreatePembimbingRequestValidator : AbstractValidator<CreatePembimbingRequest>
{
    public CreatePembimbingRequestValidator()
    {
        RuleFor(v => v.Nama)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.Nama);

        RuleFor(v => v.Nip)
            .NotEmpty()
            .Matches("^[0-9]+$")
            .WithMessage("Nip hanya boleh berisi angka.")
            .MaximumLength(MaximumLengthFor.Nip);

        RuleFor(v => v.Jabatan)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.Jabatan);

        RuleFor(v => v.Email)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.Email);

    }
}
