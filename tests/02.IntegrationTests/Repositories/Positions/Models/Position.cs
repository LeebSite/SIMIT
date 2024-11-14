namespace Pertamina.SIMIT.IntegrationTests.Repositories.Positions.Models;

public class Position
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    public IList<string> RoleNames { get; set; } = new List<string>();
    public IList<CustomParameter> CustomParameters { get; set; } = new List<CustomParameter>();
}
