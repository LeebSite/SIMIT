using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Shared.Logbooks.Commands.ApprovalLogbook;
public class ApproveMultiLogbooksRequest
{
    [OpenApiContentType(ContentTypes.ApplicationJson)]
    public IList<ApproveLogbooksRequest> Logbooks { get; set; } = new List<ApproveLogbooksRequest>();
}
public class ApproveMultiLogbooksRequestValidator : AbstractValidator<ApproveMultiLogbooksRequest>
{
    public ApproveMultiLogbooksRequestValidator()
    {
        RuleFor(v => v.Logbooks.Count)
           .GreaterThan(0);
    }
}

