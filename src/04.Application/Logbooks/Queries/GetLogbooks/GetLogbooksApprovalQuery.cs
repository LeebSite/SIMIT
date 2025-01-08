using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Extensions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Enums;

using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Commands.GetLogbook;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooks;

namespace Pertamina.SIMIT.Application.Logbooks.Queries.GetLogbooks;
public class GetLogbooksApprovalQuery : GetLogbookRequest, IRequest<PaginatedListResponse<GetLogbooksLogbook>>
{
}
public class GetLogbooksApprovalQueryValidator : AbstractValidator<GetLogbooksApprovalQuery>
{
    public GetLogbooksApprovalQueryValidator()
    {
        Include(new GetLogbookRequestValidator());
    }
}

public class GetLogbooksLogbookApprovalMapping : IMapFrom<Logbook, GetLogbooksLogbook>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Logbook, GetLogbooksLogbook>()
            .ForMember(dest => dest.MahasiswaId, opt => opt.MapFrom(src => src.MahasiswaId))
            .ForMember(dest => dest.LogbookAttachmentId, opt => opt.MapFrom(src =>
                src.Attachments.FirstOrDefault() != null ? src.Attachments.FirstOrDefault().Id : (Guid?)null));
    }
}

public class GetLogbooksApprovalQueryHandler : IRequestHandler<GetLogbooksApprovalQuery, PaginatedListResponse<GetLogbooksLogbook>>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStorageService _storageService;

    public GetLogbooksApprovalQueryHandler(ISIMITDbContext context, IMapper mapper, IStorageService storageService)
    {
        _context = context;
        _mapper = mapper;
        _storageService = storageService;
    }
    public async Task<PaginatedListResponse<GetLogbooksLogbook>> Handle(GetLogbooksApprovalQuery request, CancellationToken cancellationToken)
    {

        var query = _context.Logbooks
            .Include(m => m.Mahasiswa)
            .Include(m => m.Attachments)
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            //.Where(x => !x.Status)
            .Where(x => x.Mahasiswa.Pembimbing.Email == request.User)
            .ApplySearch(request.SearchText, typeof(GetLogbooksLogbook), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
                typeof(GetLogbooksLogbook),
                _mapper.ConfigurationProvider,
                nameof(GetLogbooksLogbook.LogbookDate),
                SortOrder.Desc);

        // Apply search if any
        //if (!string.IsNullOrEmpty(request.SearchText))
        //{
        //    query = query.ApplySearch(request.SearchText, typeof(GetLogbooksLogbook), _mapper.ConfigurationProvider);
        //}

        // Apply sorting
        //query = query.ApplyOrder(
        //    request.SortField,
        //    request.SortOrder,
        //    typeof(GetLogbooksLogbook),
        //    _mapper.ConfigurationProvider,
        //    nameof(GetLogbooksLogbook.MahasiswaId),
        //    SortOrder.Asc);

        // Paginate results
        //var result = await query.ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        var result = await query
               .ProjectTo<GetLogbooksLogbook>(_mapper.ConfigurationProvider)
               .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        var attachmentIds = result.Items.Select(item => item.LogbookAttachmentId).Distinct().ToList();
        var attachments = await _context.LogbookAttachments
            .Where(a => attachmentIds.Contains(a.Id))
            .ToDictionaryAsync(a => a.Id);

        foreach (var item in result.Items)
        {
            var attachment = _context.LogbookAttachments.FirstOrDefault(a => a.Id == item.LogbookAttachmentId);
            if (attachment == null)
            {
                Console.WriteLine($"Attachment not found for Logbook ID: {item.LogbookId}");
                continue;
            }

            if (string.IsNullOrEmpty(attachment.FileContentType) || string.IsNullOrEmpty(attachment.StorageFileId))
            {
                Console.WriteLine($"Invalid attachment data for Logbook ID: {item.LogbookId}. ContentType: {attachment.FileContentType}, StorageFileId: {attachment.StorageFileId}");
            }

            item.FileName = attachment.FileName;
            item.ContentType = attachment.FileContentType; // Set default jika null
            item.Content = await _storageService.ReadAsync(attachment.StorageFileId);
        }

        // Map the paginated result to the response DTO
        return result.ToPaginatedListResponse();
    }
}
