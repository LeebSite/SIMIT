using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbing;

namespace Pertamina.SIMIT.Application.Pembimbings.Queries.GetPembimbing;
public class GetPembimbingQuery : IRequest<GetPembimbingResponse>
{
    public Guid PembimbingId { get; set; }
}

public class GetPembimbingResponseMapping : IMapFrom<Pembimbing, GetPembimbingResponse>
{

}

public class GetPembimbingQueryHandler : IRequestHandler<GetPembimbingQuery, GetPembimbingResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetPembimbingQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetPembimbingResponse> Handle(GetPembimbingQuery request, CancellationToken cancellationToken)
    {
        var pembimbing = await _context.Pembimbings
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        return _mapper.Map<GetPembimbingResponse>(pembimbing);
    }
}
