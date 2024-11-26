using FluentValidation;
using Microsoft.AspNetCore.Http;
using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Laporans.Constants;

namespace Pertamina.SIMIT.Shared.Laporans.Commands.CreateLaporan;
public class CreateLaporanRequest
{
    //[OpenApiContentType(ContentTypes.TextPlain)]
    //public Guid MahasiswaId { get; set; }

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string MahasiswaNim { get; set; } = default!;

    [OpenApiContentType(ContentTypes.TextPlain)]
    public string Deskripsi { get; set; } = default!;

    [OpenApiContentType(ContentTypesFor.LaporanFile.Value)]
    public IFormFile File { get; set; } = default!;
}

public class CreateLaporanRequestValidator : AbstractValidator<CreateLaporanRequest>
{

    public CreateLaporanRequestValidator()
    {

        RuleFor(v => v.MahasiswaNim)
            .NotEmpty();

        RuleFor(v => v.Deskripsi)
            .NotEmpty()
            .MaximumLength(MaximumLengthFor.Deskripsi);

        RuleFor(v => v.File.ContentType);
        //.Must(HaveSupportedContentType)
        //.WithMessage($"{DisplayTextFor.Laporan} file extension is not supported. Please {CommonDisplayTextFor.Upload.ToLower()} supported file.");
    }

    //private bool HaveSupportedContentType(string contentType)
    //{
    //    return ContentTypesFor.LaporanFile.List.Contains(contentType);
    //}
}
