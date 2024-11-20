using FluentValidation;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Shared.Mahasiswas.Commands.UpdateMahasiswas;
public class UpdateMahasiswasRequest
{
    [OpenApiContentType(ContentTypes.ApplicationJson)]
    public IList<UpdateMahasiswasMahasiswa> Mahasiswas { get; set; } = new List<UpdateMahasiswasMahasiswa>();
}
public class UpdateMahasiswasRequestValidator : AbstractValidator<UpdateMahasiswasRequest>
{
    public UpdateMahasiswasRequestValidator()
    {
        RuleFor(v => v.Mahasiswas.Count)
           .GreaterThan(0);
    }
}

