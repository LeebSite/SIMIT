
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Logbooks.Commands.ApprovalLogbook;

namespace Pertamina.SIMIT.Application.Logbooks.Commands.ApprovalLogbook;
public class ApproveMultiLogbooksCommand : ApproveMultiLogbooksRequest, IRequest<ApproveLogbookResponse>
{
}

public class ApproveMultiLogbooksHandler : IRequestHandler<ApproveMultiLogbooksCommand, ApproveLogbookResponse>
{
    private readonly ISIMITDbContext _context;

    public ApproveMultiLogbooksHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<ApproveLogbookResponse> Handle(ApproveMultiLogbooksCommand request, CancellationToken cancellationToken)
    {
        var logbookIds = request.Logbooks.Select(l => l.LogbookId).ToList();

        // Fetch all logbooks that match the IDs and are not deleted
        var logbooks = await _context.Logbooks
            .Where(x => !x.IsDeleted && logbookIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        // If any logbook is missing, throw an exception
        //if (logbooks.Count != logbookIds.Count)
        //{
        //    var missingIds = logbookIds.Except(logbooks.Select(x => x.Id));
        //    throw new NotFoundException("Logbooks", string.Join(", ", missingIds));
        //}

        // Update the approval status for each logbook
        foreach (var logbook in logbooks)
        {
            logbook.Approval = true;
        }

        // Save changes to the database
        await _context.SaveChangesAsync(this, cancellationToken);

        return new ApproveLogbookResponse();
    }
}
