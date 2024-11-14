namespace Pertamina.SIMIT.Shared.Services.Authorization.Models.GetPositions;

public class GetPositionsPosition
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string PersonaType { get; set; } = default!;

    public string CompleteDetails => $"[{PersonaType}] {Name} ({Id})";
}
