using Newtonsoft.Json;
using Pertamina.SIMIT.IntegrationTests.Repositories.Positions.Models;

namespace Pertamina.SIMIT.IntegrationTests.Repositories.Positions;

public static class PositionRepository
{
    private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, nameof(Repositories), nameof(Positions), "positions.json");

    public static List<Position> Positions => JsonConvert.DeserializeObject<List<Position>>(File.ReadAllText(_filePath))!;
}
