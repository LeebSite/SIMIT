using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Logbooks.Constants;
using Pertamina.SIMIT.Shared.Logbooks.Commands.ApprovalLogbook;

namespace Pertamina.SIMIT.Application.Logbooks.Commands.ApprovalLogbook;
public class ApproveSingleLogbookCommand : ApproveLogbooksRequest, IRequest
{

}

public class ApproveSingleLogbookCommandValidator : AbstractValidator<ApproveSingleLogbookCommand>
{
    public ApproveSingleLogbookCommandValidator()
    {
        Include(new ApproveLogbooksRequestValidator());
    }
}

public class ApproveSingleLogbookCommandHandler : IRequestHandler<ApproveSingleLogbookCommand>
{
    private readonly ISIMITDbContext _context;

    public ApproveSingleLogbookCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ApproveSingleLogbookCommand request, CancellationToken cancellationToken)
    {
        var logbook = await _context.Logbooks
            .Where(x => !x.IsDeleted && x.Id == request.LogbookId)
            .SingleOrDefaultAsync(cancellationToken);

        if (logbook == null)
        {
            throw new NotFoundException(DisplayTextFor.Logbook, request.LogbookId);
        }

        if (logbook.Approval == request.Approval)
        {
            // Tidak perlu menyimpan ulang jika nilai approval sama
            return Unit.Value;
        }

        logbook.Approval = request.Approval;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }

}
