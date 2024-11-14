using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Laporans.Constants;

namespace Pertamina.SIMIT.Shared.Laporans.Commands.CreateLaporan;

public class CreateLaporanRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public string FileLaporan { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string FileProject { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Deskripsi { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string MahasiswaNim { get; set; } = default!;
}

public class CreateLaporanRequestValidator : AbstractValidator<CreateLaporanRequest>
{
    public CreateLaporanRequestValidator()
    {
        RuleFor(v => v.FileLaporan)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.FileLaporan);

        RuleFor(v => v.FileProject)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.FileProject);

        RuleFor(v => v.Deskripsi)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.Deskripsi);
    }
}
