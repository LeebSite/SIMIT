using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.CurrentUser;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;
//using Pertamina.SIMIT.Shared.MahasiswaAttachments.Constants;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Options;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;

namespace Pertamina.SIMIT.Application.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;
public class CreateMahasiswaAttachmentCommand : CreateMahasiswaAttachmentRequest, IRequest<CreateMahasiswaAttachmentResponse>
{
}

public class CreateMahasiswaAttachmentCommandValidator : AbstractValidator<CreateMahasiswaAttachmentCommand>
{
    public CreateMahasiswaAttachmentCommandValidator(IOptions<MahasiswaAttachmentOptions> mahasiswaAttachmentOptions)
    {
        Include(new CreateMahasiswaAttachmentRequestValidator(mahasiswaAttachmentOptions));
    }
}

public class CreateMahasiswaAttachmentCommandHandler : IRequestHandler<CreateMahasiswaAttachmentCommand, CreateMahasiswaAttachmentResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStorageService _storageService;

    public CreateMahasiswaAttachmentCommandHandler(ISIMITDbContext context, ICurrentUserService currentUserService, IStorageService storageService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _storageService = storageService;
    }

    public async Task<CreateMahasiswaAttachmentResponse> Handle(CreateMahasiswaAttachmentCommand request, CancellationToken cancellationToken)
    {
        var mahasiswa = await _context.Mahasiswas
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.Id == request.MahasiswaId)
            .Include(a => a.Attachments.Where(x => !x.IsDeleted))
            .SingleOrDefaultAsync(cancellationToken);

        if (mahasiswa is null)
        {
            throw new NotFoundException(DisplayTextFor.Mahasiswa, request.MahasiswaId);
        }

        var mahasiswaAttachmentWithTheSameFileName = mahasiswa.Attachments
            .Where(x => x.FileName == request.File.FileName)
            .SingleOrDefault();

        if (mahasiswaAttachmentWithTheSameFileName is not null)
        {
            throw new AlreadyExistsExceptions(Shared.MahasiswaAttachments.Constants.DisplayTextFor.Attachment, CommonDisplayTextFor.FileName, request.File.FileName);
        }

        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        var file = memoryStream.ToArray();

        var mahasiswaAttachment = new MahasiswaAttachment
        {
            Id = Guid.NewGuid(),
            MahasiswaId = mahasiswa.Id,
            FileName = request.File.FileName,
            FileSize = request.File.Length,
            FileContentType = request.File.ContentType,
            StorageFileId = await _storageService.CreateAsync(file)
        };

        //mahasiswaAttachment.DomainEvents.Add(new MahasiswaAttachmentCreatedEvent(mahasiswaAttachment.Id))
        await _context.MahasiswaAttachments.AddAsync(mahasiswaAttachment, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new CreateMahasiswaAttachmentResponse
        {
            MahasiswaAttachmentId = mahasiswaAttachment.Id
        };
    }
}
