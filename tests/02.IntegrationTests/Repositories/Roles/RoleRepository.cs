using Newtonsoft.Json;
using Pertamina.SIMIT.IntegrationTests.Repositories.Roles.Models;

namespace Pertamina.SIMIT.IntegrationTests.Repositories.Roles;

public static class RoleRepository
{
    private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, nameof(Repositories), nameof(Roles), "roles.json");

    public static List<Role> Roles => JsonConvert.DeserializeObject<List<Role>>(File.ReadAllText(_filePath))!;
}
