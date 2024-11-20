
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
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswas;

namespace Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswasQuery;
public class GetMahasiswasQuery : PaginatedListRequest, IRequest<PaginatedListResponse<GetMahasiswasMahasiswa>>
{
}

public class GetMahasiswasQueryValidator : AbstractValidator<GetMahasiswasQuery>
{
    public GetMahasiswasQueryValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}

public class GetMahasiswasMahasiswaMapping : IMapFrom<Mahasiswa, GetMahasiswasMahasiswa>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Mahasiswa, GetMahasiswasMahasiswa>();

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
            .AsNoTracking()
            .Where(m => !m.IsDeleted);

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

        // Map the paginated result to the response DTO
        return result.AsPaginatedListResponse<GetMahasiswasMahasiswa, Mahasiswa>(_mapper.ConfigurationProvider);
    }
}

