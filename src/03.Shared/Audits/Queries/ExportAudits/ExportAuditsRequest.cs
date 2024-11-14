using Pertamina.SIMIT.Shared.Common.Attributes;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Shared.Audits.Queries.ExportAudits;

public class ExportAuditsRequest
{
    [OpenApiContentType(ContentTypes.TextPlain)]
    public IList<Guid> AuditIds { get; set; } = new List<Guid>();
}
