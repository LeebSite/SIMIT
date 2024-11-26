using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Shared.Laporans.Constants;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporan;

namespace Pertamina.SIMIT.Application.Laporans.Queries.GetLaporan;
public class GetLaporanQuery : IRequest<GetLaporanResponse>
{
    public Guid LaporanId { get; set; }
}

public class GetLaporanHandler : IRequestHandler<GetLaporanQuery, GetLaporanResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IStorageService _storageService;

    public GetLaporanHandler(ISIMITDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<GetLaporanResponse> Handle(GetLaporanQuery request, CancellationToken cancellationToken)
    {
        var laporan = await _context.Laporans
            .Where(x => !x.IsDeleted && x.Id == request.LaporanId)
            .SingleOrDefaultAsync(cancellationToken);

        if (laporan is null)
        {
            throw new NotFoundException(DisplayTextFor.Laporan, request.LaporanId);
        }

        var response = new GetLaporanResponse
        {
            FileName = $"{laporan.FileName}",
            ContentType = laporan.FileContentType,
            Content = await _storageService.ReadAsync(laporan.StorageFileId)
        };

        return response;
    }
}
