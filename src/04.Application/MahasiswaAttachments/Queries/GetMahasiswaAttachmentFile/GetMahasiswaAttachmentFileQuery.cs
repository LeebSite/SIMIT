using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Constants;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Queries.GetMahasiswaAttachmentFile;

namespace Pertamina.SIMIT.Application.MahasiswaAttachments.Queries.GetMahasiswaAttachmentFile;
public class GetMahasiswaAttachmentFileQuery : IRequest<GetMahasiswaAttachmentFileResponse>
{
    public Guid MahasiswaAttachmentId { get; set; }
}
public class GetMahasiswaAttachmentFileHandler : IRequestHandler<GetMahasiswaAttachmentFileQuery, GetMahasiswaAttachmentFileResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IStorageService _storageService;

    public GetMahasiswaAttachmentFileHandler(ISIMITDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<GetMahasiswaAttachmentFileResponse> Handle(GetMahasiswaAttachmentFileQuery request, CancellationToken cancellationToken)
    {
        var mahasiswaAttachment = await _context.MahasiswaAttachments
            .Where(x => !x.IsDeleted && x.Id == request.MahasiswaAttachmentId)
            .SingleOrDefaultAsync(cancellationToken);

        if (mahasiswaAttachment is null)
        {
            throw new NotFoundException(DisplayTextFor.MahasiswaAttachment, request.MahasiswaAttachmentId);
        }

        var response = new GetMahasiswaAttachmentFileResponse
        {
            FileName = $"{mahasiswaAttachment.FileName}",
            ContentType = mahasiswaAttachment.FileContentType,
            Content = await _storageService.ReadAsync(mahasiswaAttachment.StorageFileId)
        };

        return response;
    }
}
