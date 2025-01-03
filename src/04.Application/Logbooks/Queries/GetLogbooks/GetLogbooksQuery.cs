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
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooks;

namespace Pertamina.SIMIT.Application.Logbooks.Queries.GetLogbooksQuery;
public class GetLogbooksQuery : PaginatedListRequest, IRequest<PaginatedListResponse<GetLogbooksLogbook>>
{
}
public class GetLogbooksQueryValidator : AbstractValidator<GetLogbooksQuery>
{
    public GetLogbooksQueryValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}

public class GetLogbooksLogbookMapping : IMapFrom<Logbook, GetLogbooksLogbook>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Logbook, GetLogbooksLogbook>();
    }
}

public class GetLogbooksQueryHandler : IRequestHandler<GetLogbooksQuery, PaginatedListResponse<GetLogbooksLogbook>>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetLogbooksQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedListResponse<GetLogbooksLogbook>> Handle(GetLogbooksQuery request, CancellationToken cancellationToken)
    {

        var query = _context.Logbooks
            .Include(m => m.Mahasiswa)
            .AsNoTracking()
            .Where(m => !m.IsDeleted);

        // Apply search if any
        if (!string.IsNullOrEmpty(request.SearchText))
        {
            query = query.ApplySearch(request.SearchText, typeof(GetLogbooksLogbook), _mapper.ConfigurationProvider);
        }

        // Apply sorting
        query = query.ApplyOrder(
            request.SortField,
            request.SortOrder,
            typeof(GetLogbooksLogbook),
            _mapper.ConfigurationProvider,
            nameof(GetLogbooksLogbook.MahasiswaId),
            SortOrder.Asc);

        // Paginate results
        var result = await query.ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        // Map the paginated result to the response DTO
        return result.AsPaginatedListResponse<GetLogbooksLogbook, Logbook>(_mapper.ConfigurationProvider);
    }
}
