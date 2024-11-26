using FluentValidation;
using Microsoft.AspNetCore.Http;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Laporans.Constants;

namespace Pertamina.SIMIT.Shared.Laporans.Commands.UpdateLaporan;
public class UpdateLaporanRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public Guid LaporanId { get; set; }

    [OpenApiContentType(ContentTypes.TextPlain)]
    public Guid MahasiswaId { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string MahasiswaNim { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Deskripsi { get; set; } = default!;

    [OpenApiContentType(ContentTypesFor.LaporanFile.Value)]
    public IFormFile File { get; set; } = default!;
}

public class UpdateLaporanRequestValidator : AbstractValidator<UpdateLaporanRequest>
{
    public UpdateLaporanRequestValidator()
    {
        RuleFor(v => v.LaporanId)
            .NotEmpty();

        RuleFor(v => v.MahasiswaNim)
            .NotEmpty();
    }
}
