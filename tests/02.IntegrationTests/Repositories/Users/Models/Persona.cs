namespace Pertamina.SIMIT.IntegrationTests.Repositories.Users.Models;

public class Persona
{
    public string Type { get; set; } = default!;

    public IList<string> PositionIds { get; set; } = new List<string>();
}
