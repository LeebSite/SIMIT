
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Extensions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Enums;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.GetMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswas;

namespace Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswasQuery;
public class GetMahasiswasQuery : GetMahasiswaRequest, IRequest<PaginatedListResponse<GetMahasiswasMahasiswa>>
{
}

public class GetMahasiswasQueryValidator : AbstractValidator<GetMahasiswasQuery>
{
    public GetMahasiswasQueryValidator()
    {
        Include(new GetMahasiswaRequestValidator());
    }
}

public class GetMahasiswasMahasiswaMapping : IMapFrom<Mahasiswa, GetMahasiswasMahasiswa>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Mahasiswa, GetMahasiswasMahasiswa>()
            .ForMember(dest => dest.LaporanId, opt => opt.MapFrom(src =>
                src.Laporans.FirstOrDefault() != null ? src.Laporans.FirstOrDefault().Id : (Guid?)null))
            .ForMember(dest => dest.LaporanDeskripsi, opt => opt.MapFrom(src =>
                src.Laporans.FirstOrDefault() != null ? src.Laporans.FirstOrDefault().Deskripsi : null));
    }
}

public class GetMahasiswasQueryHandler : IRequestHandler<GetMahasiswasQuery, PaginatedListResponse<GetMahasiswasMahasiswa>>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetMahasiswasQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedListResponse<GetMahasiswasMahasiswa>> Handle(GetMahasiswasQuery request, CancellationToken cancellationToken)
    {

        var query = _context.Mahasiswas
            .Include(m => m.Pembimbing)
            .Include(m => m.Laporans)
            .AsNoTracking()
            .Where(m => !m.IsDeleted)
            .Where(x => string.IsNullOrEmpty(request.Kampus) || x.Kampus == request.Kampus)
            .Where(x => string.IsNullOrEmpty(request.Bagian) || x.Bagian == request.Bagian);

        // Apply search if any
        if (!string.IsNullOrEmpty(request.SearchText))
        {
            query = query.ApplySearch(request.SearchText, typeof(GetMahasiswasMahasiswa), _mapper.ConfigurationProvider);
        }

        // Apply sorting
        query = query.ApplyOrder(
            request.SortField,
            request.SortOrder,
            typeof(GetMahasiswasMahasiswa),
            _mapper.ConfigurationProvider,
            nameof(GetMahasiswasMahasiswa.Nama),
            SortOrder.Asc);

        // Paginate results
        var result = await query.ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        // Ambil nilai distinct untuk dropdown
        var kampusList = await _context.Mahasiswas
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.Kampus)
            .Distinct()
            .ToListAsync(cancellationToken);

        var bagianList = await _context.Mahasiswas
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.Bagian)
            .Distinct()
            .ToListAsync(cancellationToken);

        // Tambahkan nilai distinct ke dalam response (jika diperlukan di response utama)
        var response = result.AsPaginatedListResponse<GetMahasiswasMahasiswa, Mahasiswa>(_mapper.ConfigurationProvider);
        //response.Metadata["KampusList"] = kampusList;
        //response.Metadata["BagianList"] = bagianList;

        return response;
    }
}

