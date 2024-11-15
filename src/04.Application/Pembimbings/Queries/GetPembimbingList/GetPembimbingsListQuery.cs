
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Extensions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbingsList;

namespace Pertamina.SIMIT.Application.Pembimbings.Queries.GetPembimbingList;
public class GetPembimbingsListQuery : IRequest<ListResponse<GetPembimbingsList>>
{
}

public class GetPembimbingsListMahasiswaMapping : IMapFrom<Pembimbing, GetPembimbingsList>
{
}

public class GetPembimbingsListQueryHandler : IRequestHandler<GetPembimbingsListQuery, ListResponse<GetPembimbingsList>>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetPembimbingsListQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListResponse<GetPembimbingsList>> Handle(GetPembimbingsListQuery request, CancellationToken cancellationToken)
    {
        var apps = await _context.Pembimbings
            .AsNoTracking()
            .OrderBy(x => x.Nama)
            .ProjectTo<GetPembimbingsList>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return apps.ToListResponse();
    }
}

