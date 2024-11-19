using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbings;
public class UpdatePembimbingsRequest
{
    [OpenApiContentType(ContentTypes.ApplicationJson)]
    public IList<UpdatePembimbingsPembimbing> Pembimbings { get; set; } = new List<UpdatePembimbingsPembimbing>();

}
public class UpdatePembimbingsRequestValidator : AbstractValidator<UpdatePembimbingsRequest>
{
    public UpdatePembimbingsRequestValidator()
    {
        RuleFor(v => v.Pembimbings.Count)
           .GreaterThan(0);
    }
}

