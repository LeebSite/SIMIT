using Newtonsoft.Json;
using Pertamina.SIMIT.IntegrationTests.Repositories.Users.Models;

namespace Pertamina.SIMIT.IntegrationTests.Repositories.Users;

public static class UserRepository
{
    private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, nameof(Repositories), nameof(Users), "users.json");

    public static List<User> Users => JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(_filePath))!;
}
