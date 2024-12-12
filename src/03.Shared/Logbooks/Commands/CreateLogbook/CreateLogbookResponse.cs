
using Pertamina.SIMIT.Base.ValueObjects;
using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Logbooks.Commands.CreateLogbook;
public class CreateLogbookResponse : Response
{
    public Guid LogbookId { get; set; }
    public Geolocation? FromGeolocation { get; set; }
}
