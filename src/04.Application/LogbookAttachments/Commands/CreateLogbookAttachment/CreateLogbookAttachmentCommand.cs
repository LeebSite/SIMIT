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
using Pertamina.SIMIT.Shared.LogbookAttachments.CreateLogbookAttachments;
using Pertamina.SIMIT.Shared.LogbookAttachments.Options;
using Pertamina.SIMIT.Shared.Logbooks.Constants;

namespace Pertamina.SIMIT.Application.LogbookAttachments.Commands.CreateTicketAttachment;
public class CreateLogbookAttachmentCommand : CreateLogbookAttachmentRequest, IRequest<CreateLogbookAttachmentResponse>
{
}
public class CreateTicketAttachmentCommandValidator : AbstractValidator<CreateLogbookAttachmentCommand>
{
    public CreateTicketAttachmentCommandValidator(IOptions<LogbookAttachmentOptions> logbookAttachmentOptions)
    {
        Include(new CreateLogbookAttachmentRequestValidator(logbookAttachmentOptions));
    }
}
public class CreateLogbookAttachmentCommandHandler : IRequestHandler<CreateLogbookAttachmentCommand, CreateLogbookAttachmentResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IStorageService _storageService;

    public CreateLogbookAttachmentCommandHandler(ISIMITDbContext context, ICurrentUserService currentUser, IStorageService storageService)
    {
        _context = context;
        _currentUser = currentUser;
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

        //if (logbook.CreatorId != _currentUser.UserId)
        //{
        //    throw new ForbiddenAccessException($"User {_currentUser.Username} is not the {DisplayTextFor.Creator} of {DisplayTextFor.Ticket} {ticket.Id}.");
        //}

        //if (logbook.Status != LogbookStatus.Draft)
        //{
        //    throw new InvalidOperationException(
        //        @$"Cannot {CommonDisplayTextFor.Update.ToLower()} {DisplayTextFor.Ticket}
        //            with {DisplayTextFor.Code} {ticket.Code} because
        //            its {DisplayTextFor.Status.ToLower()}
        //            is not {TicketStatus.Draft.GetDescription()}.");
        //}

        var logbookAttachmentWithTheSameFileName = logbook.Attachments
            .Where(x => x.FileName == request.File.FileName)
            .SingleOrDefault();

        if (logbookAttachmentWithTheSameFileName is not null)
        {
            throw new AlreadyExistsExceptions(Shared.LogbookAttachments.Constants.DisplayTextFor.Attachment, CommonDisplayTextFor.FileName, request.File.FileName);
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
            StorageFileId = await _storageService.CreateAsync(file),
        };

        //logbookAttachment.DomainEvents.Add(new LogbookAttachmentCreatedEvent(logbookAttachment.Id));

        await _context.LogbookAttachments.AddAsync(logbookAttachment, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new CreateLogbookAttachmentResponse
        {
            LogbookAttachmentId = logbookAttachment.Id
        };
    }
}
