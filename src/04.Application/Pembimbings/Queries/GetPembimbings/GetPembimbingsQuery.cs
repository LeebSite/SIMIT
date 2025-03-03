﻿using AutoMapper;
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
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbings;

namespace Pertamina.SIMIT.Application.Pembimbings.Queries.GetPembimbings;
public class GetPembimbingsQuery : PaginatedListRequest, IRequest<PaginatedListResponse<GetPembimbingsPembimbing>>
{
}

public class GetPembimbingsResponseMapping : IMapFrom<Pembimbing, GetPembimbingsPembimbing>
{

}

public class GetPembimbingsQueryValidator : AbstractValidator<GetPembimbingsQuery>
{
    public GetPembimbingsQueryValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
//public class GetPembimbingsQueryHandler : IRequestHandler<GetPembimbingsQuery, PaginatedListResponse<GetPembimbingsPembimbing>>
//{
//    private readonly ISIMITDbContext _context;
//    private readonly IMapper _mapper;

//    public GetPembimbingsQueryHandler(ISIMITDbContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public async Task<PaginatedListResponse<GetPembimbingsPembimbing>> Handle(GetPembimbingsQuery request, CancellationToken cancellationToken)
//    {
//        var query = _context.Pembimbings
//           .AsNoTracking();

//        // Apply search if any
//        if (!string.IsNullOrEmpty(request.SearchText))
//        {
//            query = query.ApplySearch(request.SearchText, typeof(GetPembimbingsPembimbing), _mapper.ConfigurationProvider);
//        }

//        // Apply sorting
//        query = query.ApplyOrder(
//            request.SortField,
//            request.SortOrder,
//            typeof(GetPembimbingsPembimbing),
//            _mapper.ConfigurationProvider,
//            nameof(GetPembimbingsPembimbing.Nama),
//            SortOrder.Asc);

//        // Paginate results
//        var result = await query.ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

//        // Map the paginated result to the response DTO
//        return result.AsPaginatedListResponse<GetPembimbingsPembimbing, Pembimbing>(_mapper.ConfigurationProvider);

//        //var query = _context.Pembimbings
//        //   .AsNoTracking()
//        //   .ApplySearch(request.SearchText, typeof(GetPembimbingsPembimbing), _mapper.ConfigurationProvider);
//        ////.ApplyOrder(request.SortField, request.SortOrder,
//        ////    typeof(GetPembimbingsPembimbing),
//        ////    _mapper.ConfigurationProvider,
//        ////    nameof(GetPembimbingsPembimbing.Nama),
//        //var result = await query.ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

//        //return result.AsPaginatedListResponse<GetPembimbingsPembimbing, Pembimbing>(_mapper.ConfigurationProvider);

//        //var query = _context.Pembimbings
//        //   .AsNoTracking()
//        //   .ApplySearch(request.SearchText, typeof(GetPembimbingsPembimbing), _mapper.ConfigurationProvider);

//        //var result = await query
//        //    .ProjectTo<GetPembimbingsPembimbing>(_mapper.ConfigurationProvider)
//        //    .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

//        //return result.ToPaginatedListResponse();
//    }
//}

public class GetPembimbingsQueryHandler : IRequestHandler<GetPembimbingsQuery, PaginatedListResponse<GetPembimbingsPembimbing>>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;

    public GetPembimbingsQueryHandler(ISIMITDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetPembimbingsPembimbing>> Handle(GetPembimbingsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Pembimbings
            .AsNoTracking()
            .ApplySearch(request.SearchText, typeof(GetPembimbingsPembimbing), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
                typeof(GetPembimbingsPembimbing),
                _mapper.ConfigurationProvider,
                nameof(GetPembimbingsPembimbing.Nama),
                SortOrder.Asc);

        var result = await query.ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.AsPaginatedListResponse<GetPembimbingsPembimbing, Pembimbing>(_mapper.ConfigurationProvider);
    }
}
