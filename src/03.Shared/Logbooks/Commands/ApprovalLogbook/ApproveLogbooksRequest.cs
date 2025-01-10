using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Shared.Logbooks.Commands.ApprovalLogbook;
public class ApproveLogbooksRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public Guid LogbookId { get; set; }

    [OpenApiContentType(ContentTypes.TextPlain)]
    public bool Approval { get; set; } = default!;
}

public class ApproveLogbooksRequestValidator : AbstractValidator<ApproveLogbooksRequest>
{
    public ApproveLogbooksRequestValidator()
    {
        RuleFor(v => v.Approval)
         .NotEmpty();
    }
}

