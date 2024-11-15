
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Extensions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswasList;

namespace Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswasListQuery;
public class GetMahasiswasListQuery : IRequest<ListResponse<GetMahasiswasList>>
{
}

public class GetMahasiswasListMahasiswaMapping : IMapFrom<Mahasiswa, GetMahasiswasList>
{
}

public class GetMahasiswasListQueryHandler : IRequestHandler<GetMahasiswasListQuery, ListResponse<GetMahasiswasList>>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetMahasiswasListQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListResponse<GetMahasiswasList>> Handle(GetMahasiswasListQuery request, CancellationToken cancellationToken)
    {
        var apps = await _context.Mahasiswas
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Nama)
            .ProjectTo<GetMahasiswasList>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return apps.ToListResponse();
    }
}
