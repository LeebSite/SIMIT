using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Extensions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooksList;

namespace Pertamina.SIMIT.Application.Logbooks.Queries.GetLogbooksListQuery;
public class GetLogbooksListQuery : IRequest<ListResponse<GetLogbooksList>>
{
}

public class GetLogbooksListLogbookMapping : IMapFrom<Logbook, GetLogbooksList>
{
}

public class GetLogbooksListQueryHandler : IRequestHandler<GetLogbooksListQuery, ListResponse<GetLogbooksList>>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetLogbooksListQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ListResponse<GetLogbooksList>> Handle(GetLogbooksListQuery request, CancellationToken cancellationToken)
    {
        var apps = await _context.Logbooks
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.LogbookDate)
            .ProjectTo<GetLogbooksList>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return apps.ToListResponse();
    }
}
