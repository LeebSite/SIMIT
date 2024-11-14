namespace Pertamina.SIMIT.IntegrationTests.Repositories.Roles.Models;

public class Role
{
    public string Name { get; set; } = default!;

    public IList<string> Scopes { get; set; } = new List<string>();
}
