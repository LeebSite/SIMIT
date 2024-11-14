namespace Pertamina.SIMIT.Bsui.Services.FrontEnd;

public class FrontEndOptions
{
    public const string SectionKey = nameof(FrontEnd);

    public string BasePath { get; set; } = default!;
    public bool DisplayTechnicalInformation { get; set; }
}
