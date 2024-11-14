using Pertamina.SIMIT.Base.ValueObjects;

namespace Pertamina.SIMIT.Application.Services.CurrentUser;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string Username { get; }
    string ClientId { get; }
    string? PositionId { get; }
    string IpAddress { get; }
    Geolocation? Geolocation { get; }
}
