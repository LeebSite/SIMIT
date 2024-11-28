using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Extensions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Enums;
using Pertamina.SIMIT.Shared.Common.Requests;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporans;

namespace Pertamina.SIMIT.Application.Laporans.Queries.GetLaporans;
public class GetLaporansQuery : PaginatedListRequest, IRequest<PaginatedListResponse<GetLaporansLaporan>>
{
}

public class GetLaporansQueryValidator : AbstractValidator<GetLaporansQuery>
{
    public GetLaporansQueryValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}

public class GetLaporansLaporanMapping : IMapFrom<Laporan, GetLaporansLaporan>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Laporan, GetLaporansLaporan>();
        //.ForMember(dest => dest.MahasiswaNama, opt => opt.MapFrom(src => src.Mahasiswa.Nama))
        //.ForMember(dest => dest.MahasiswaNim, opt => opt.MapFrom(src => src.Mahasiswa.Nim))
        //.ForMember(dest => dest.MahasiswaKampus, opt => opt.MapFrom(src => src.Mahasiswa.Kampus));
    }
}

public class GetLaporansQueryHandler : IRequestHandler<GetLaporansQuery, PaginatedListResponse<GetLaporansLaporan>>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetLaporansQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedListResponse<GetLaporansLaporan>> Handle(GetLaporansQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Laporans
            .Include(m => m.Mahasiswa)
            .AsNoTracking()
            .Where(m => !m.IsDeleted);

        // Apply specific filters
        if (!string.IsNullOrEmpty(request.SearchText))
        {
            query = query.Where(x => x.Mahasiswa.Nim.Contains(request.SearchText));
        }

        // Apply sorting
        query = query.ApplyOrder(
            request.SortField,
            request.SortOrder,
            typeof(GetLaporansLaporan),
            _mapper.ConfigurationProvider,
            nameof(GetLaporansLaporan.MahasiswaNim),
            SortOrder.Asc);

        // Paginate results
        var result = await query.ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        // Map the paginated result to the response DTO
        return result.AsPaginatedListResponse<GetLaporansLaporan, Laporan>(_mapper.ConfigurationProvider);
    }
}
