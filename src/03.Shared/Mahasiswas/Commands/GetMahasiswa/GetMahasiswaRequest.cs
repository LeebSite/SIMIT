using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Enums;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;

namespace Pertamina.SIMIT.Shared.Mahasiswas.Commands.GetMahasiswa;
public class GetMahasiswaRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public int Page { get; set; } = 1;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public int PageSize { get; set; } = 10;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string? SearchText { get; set; }

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string? SortField { get; set; }

    [OpenApiContentType(ContentTypes.TextPlain)]
    public SortOrder? SortOrder { get; set; }

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Kampus { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Bagian { get; set; } = default!;
}

public class GetMahasiswaRequestValidator : AbstractValidator<GetMahasiswaRequest>
{
    public GetMahasiswaRequestValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThan(0);

        RuleFor(v => v.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(v => v.SortOrder)
            .IsInEnum();

        RuleFor(v => v.Kampus)
          .NotEmpty()
          .MaximumLength(MaximumLengthFor.Kampus);

        RuleFor(v => v.Bagian)
          .NotEmpty()
          .MaximumLength(MaximumLengthFor.Bagian);
    }
}

