using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Logbooks.Constants;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbook;

namespace Pertamina.SIMIT.Application.Logbooks.Queries.GetLogbook;
public class GetLogbookQuery : IRequest<GetLogbookResponse>
{
    public Guid LogbookId { get; set; }
}

public class GetLogbookResponseMapping : IMapFrom<Logbook, GetLogbookResponse>
{
}

public class GetLogbookQueryHandler : IRequestHandler<GetLogbookQuery, GetLogbookResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetLogbookQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetLogbookResponse> Handle(GetLogbookQuery request, CancellationToken cancellationToken)
    {
        var logbook = await _context.Logbooks
            .AsNoTracking()
            .Include(m => m.Mahasiswa)
            .Where(x => !x.IsDeleted && x.Id == request.LogbookId)
            .SingleOrDefaultAsync(cancellationToken);

        if (logbook is null)
        {
            throw new NotFoundException(DisplayTextFor.Logbook, request.LogbookId);
        }

        var response = _mapper.Map<GetLogbookResponse>(logbook);

        return response;
    }
}
