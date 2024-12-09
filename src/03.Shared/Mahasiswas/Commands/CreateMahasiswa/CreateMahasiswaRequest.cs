
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Constants;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;

namespace Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
public class CreateMahasiswaRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Nama { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Nim { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Kampus { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]

    public DateTime? MulaiMagang { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public DateTime? SelesaiMagang { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Bagian { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public Guid PembimbingId { get; set; } = default!;

    [OpenApiContentType(ContentTypesFor.MahasiswaAttachmentFile.Value)]
    public IFormFile File { get; set; } = default!;

}

public class CreateMahasiswaRequestValidator : AbstractValidator<CreateMahasiswaRequest>
{
    public CreateMahasiswaRequestValidator()
    {
        RuleFor(v => v.Nama)
          .NotEmpty()
          .MaximumLength(MaximumLengthFor.Nama);

        RuleFor(v => v.Nim)
          .NotEmpty()
          .MaximumLength(MaximumLengthFor.Nim);

        RuleFor(v => v.Kampus)
          .NotEmpty()
          .MaximumLength(MaximumLengthFor.Kampus);

        RuleFor(v => v.MulaiMagang)
         .NotEmpty()
         .Must(date => date != DateTime.MinValue)
         .WithMessage("Tanggal mulai magang harus diisi.");

        RuleFor(v => v.SelesaiMagang)
          .NotEmpty()
          .Must(date => date != DateTime.MinValue)
          .WithMessage("Tanggal selesai magang harus diisi.");

        RuleFor(v => v.Bagian)
          .NotEmpty()
          .MaximumLength(MaximumLengthFor.Bagian);

        RuleFor(v => v.PembimbingId)
         .NotEmpty();

        //RuleFor(v => v.MahasiswaId)
        // .NotEmpty();

        RuleFor(v => v.File)
         .NotEmpty();
    }
}

