namespace Pertamina.SIMIT.Shared.Services.Authorization.Models.GetPositions;

public class GetPositionsResponse
{
    public IList<GetPositionsPosition> Positions { get; set; } = new List<GetPositionsPosition>();
}
