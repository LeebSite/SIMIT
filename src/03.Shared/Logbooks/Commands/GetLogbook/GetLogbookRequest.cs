using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Enums;

namespace Pertamina.SIMIT.Shared.Logbooks.Commands.GetLogbook;
public class GetLogbookRequest
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
    public string User { get; set; }
}

public class GetLogbookRequestValidator : AbstractValidator<GetLogbookRequest>
{
    public GetLogbookRequestValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThan(0);

        RuleFor(v => v.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(v => v.SortOrder)
            .IsInEnum();
    }
}
