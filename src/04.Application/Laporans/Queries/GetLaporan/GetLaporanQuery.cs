
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Laporans.Constants;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporan;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;

namespace Pertamina.SIMIT.Application.Laporans.Queries.GetLaporan;
public class GetLaporanQuery : IRequest<GetLaporanResponse>
{
    public Guid LaporanId { get; set; }
}

public class GetLaporanResponseMapping : IMapFrom<Laporan, GetLaporanResponse>
{
}
public class GetLaporanQueryHandler : IRequestHandler<GetLaporanQuery, GetLaporanResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetLaporanQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetLaporanResponse> Handle(GetLaporanQuery request, CancellationToken cancellationToken)
    {
        var laporan = await _context.Laporans
            .AsNoTracking()
            .Include(m => m.Mahasiswa)
            .Where(x => !x.IsDeleted && x.Id == request.LaporanId)
            .SingleOrDefaultAsync(cancellationToken);

        if (laporan is null)
        {
            throw new NotFoundException(DisplayTextFor.Laporan, request.LaporanId);
        }

        var response = _mapper.Map<GetLaporanResponse>(laporan);

        return response;
    }
}
