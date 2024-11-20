using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;

namespace Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswa;
public class GetMahasiswaQuery : IRequest<GetMahasiswaResponse>
{
    public Guid MahasiswaId { get; set; }
}

public class GetMahasiswaResponseMapping : IMapFrom<Mahasiswa, GetMahasiswaResponse>
{
}
public class GetMahasiswaQueryHandler : IRequestHandler<GetMahasiswaQuery, GetMahasiswaResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetMahasiswaQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetMahasiswaResponse> Handle(GetMahasiswaQuery request, CancellationToken cancellationToken)
    {
        var mahasiswa = await _context.Mahasiswas
            .AsNoTracking()
            .Include(m => m.Pembimbing)
            .Where(x => !x.IsDeleted && x.Id == request.MahasiswaId)
            .SingleOrDefaultAsync(cancellationToken);

        if (mahasiswa is null)
        {
            throw new NotFoundException(DisplayTextFor.Mahasiswa, request.MahasiswaId);
        }

        var response = _mapper.Map<GetMahasiswaResponse>(mahasiswa);

        return response;
    }
}
