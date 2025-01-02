using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Attributes;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Audits.Constants;
using Pertamina.SIMIT.Shared.Audits.Queries.GetAudit;

namespace Pertamina.SIMIT.Application.Audits.Queries.GetAudit;

[Authorize]
public class GetAuditQuery : IRequest<GetAuditResponse>
{
    public Guid AuditId { get; set; }
}

public class GetAuditResponseMapping : IMapFrom<Audit, GetAuditResponse>
{
}

public class GetAuditQueryHandler : IRequestHandler<GetAuditQuery, GetAuditResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetAuditQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAuditResponse> Handle(GetAuditQuery request, CancellationToken cancellationToken)
    {
        var audit = await _context.Audits
            .AsNoTracking()
            .Where(x => x.Id == request.AuditId)
            .SingleOrDefaultAsync(cancellationToken);

        if (audit is null)
        {
            throw new NotFoundException(DisplayTextFor.Audit, request.AuditId);
        }

        return _mapper.Map<GetAuditResponse>(audit);
    }
}
