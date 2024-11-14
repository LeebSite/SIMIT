using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Common.Attributes;
using Pertamina.SIMIT.Application.Common.Extensions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Audits.Options;
using Pertamina.SIMIT.Shared.Audits.Queries.GetAudits;
using Pertamina.SIMIT.Shared.Common.Enums;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Services.Authorization.Constants;

namespace Pertamina.SIMIT.Application.Audits.Queries.GetAudits;

[Authorize(Policy = Permissions.SolTem_Audit_Index)]
public class GetAuditsQuery : GetAuditsRequest, IRequest<PaginatedListResponse<GetAuditsAudit>>
{
}

public class GetAuditsQueryValidator : AbstractValidator<GetAuditsQuery>
{
    public GetAuditsQueryValidator(IOptions<AuditOptions> auditOptions)
    {
        Include(new GetAuditsRequestValidator(auditOptions));
    }
}

public class GetAuditsAuditMapping : IMapFrom<Audit, GetAuditsAudit>
{
}

public class GetAuditsQueryHandler : IRequestHandler<GetAuditsQuery, PaginatedListResponse<GetAuditsAudit>>
{
    private readonly ISIMITDbContext _context;
    private readonly AuditOptions _auditOptions;
    private readonly IMapper _mapper;

    public GetAuditsQueryHandler(ISIMITDbContext context, IOptions<AuditOptions> auditOptions, IMapper mapper)
    {
        _context = context;
        _auditOptions = auditOptions.Value;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetAuditsAudit>> Handle(GetAuditsQuery request, CancellationToken cancellationToken)
    {
        var from = request.From ?? _auditOptions.FilterMinimumCreated;
        var to = request.To ?? _auditOptions.FilterMaximumCreated;

        var query = _context.Audits
            .AsNoTracking()
            .Where(x => x.Created >= from && x.Created <= to)
            .ApplySearch(request.SearchText, typeof(GetAuditsAudit), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
                typeof(GetAuditsAudit),
                _mapper.ConfigurationProvider,
                nameof(GetAuditsAudit.Created),
                SortOrder.Desc);

        var result = await query
            .ProjectTo<GetAuditsAudit>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.ToPaginatedListResponse();
    }
}
