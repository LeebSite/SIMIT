using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.CurrentUser;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.LogbookAttachments.Commands.CreateLogbookAttachment;
using Pertamina.SIMIT.Shared.LogbookAttachments.CreateLogbookAttachments;
using DisplayTextFor = Pertamina.SIMIT.Shared.Logbooks.Constants.DisplayTextFor;

namespace Pertamina.SIMIT.Application.LogbookAttachments.Commands.CreateLogbookAttachment;
public class CreateLogbookAttachmentCommand : CreateLogbookAttachmentRequest, IRequest<CreateLogbookAttachmentResponse>
{
}
public class CreateLogbookAttachmentCommandValidator : AbstractValidator<CreateLogbookAttachmentCommand>
{
    public CreateLogbookAttachmentCommandValidator()
    {
        Include(new CreateLogbookAttachmentRequestValidator());
    }
}
public class CreateLogbookAttachmentCommandHandler : IRequestHandler<CreateLogbookAttachmentCommand, CreateLogbookAttachmentResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStorageService _storageService;

    public CreateLogbookAttachmentCommandHandler(ISIMITDbContext context, ICurrentUserService currentUserService, IStorageService storageService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _storageService = storageService;
    }

    public async Task<CreateLogbookAttachmentResponse> Handle(CreateLogbookAttachmentCommand request, CancellationToken cancellationToken)
    {
        var logbook = await _context.Logbooks
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.Id == request.LogbookId)
            .Include(a => a.Attachments.Where(x => !x.IsDeleted))
            .SingleOrDefaultAsync(cancellationToken);

        if (logbook is null)
        {
            throw new NotFoundException(DisplayTextFor.Logbook, request.LogbookId);
        }

        var logbookAttachmentWithTheSameFileName = logbook.Attachments
            .Where(x => x.FileName == request.File.FileName)
            .SingleOrDefault();

        if (logbookAttachmentWithTheSameFileName is not null)
        {
            throw new AlreadyExistsExceptions(Shared.LogbookAttachments.Constants.DisplayTextFor.DisplayTextFor.Attachment, CommonDisplayTextFor.FileName, request.File.FileName);
        }

        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        var file = memoryStream.ToArray();

        var logbookAttachment = new LogbookAttachment
        {
            Id = Guid.NewGuid(),
            LogbookId = logbook.Id,
            FileName = request.File.FileName,
            FileSize = request.File.Length,
            FileContentType = request.File.ContentType,
            StorageFileId = await _storageService.CreateAsync(file)
        };

        //logbookAttachment.DomainEvents.Add(new LogbookAttachmentCreatedEvent(logbookAttachment.Id))
        await _context.LogbookAttachments.AddAsync(logbookAttachment, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new CreateLogbookAttachmentResponse
        {
            LogbookAttachmentId = logbookAttachment.Id
        };
    }
}

