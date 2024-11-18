using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Pembimbings.Constants;

namespace Pertamina.SIMIT.Application.Pembimbings.Commands.DeletePembimbing;
public class DeletePembimbingCommand : IRequest
{
    public Guid PembimbingId { get; set; }
}

public class DeletePembimbingCommandHandler : IRequestHandler<DeletePembimbingCommand>
{
    private readonly ISIMITDbContext _context;

    public DeletePembimbingCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePembimbingCommand request, CancellationToken cancellationToken)
    {
        var pembimbing = await _context.Pembimbings
            .Where(x => x.Id == request.PembimbingId)
            .SingleOrDefaultAsync(cancellationToken);

        if (pembimbing is null)
        {
            throw new NotFoundException(DisplayTextFor.Pembimbing, request.PembimbingId);
        }

        //if (pembimbing.Tickets.Any())
        //{
        //    throw new InvalidOperationException($"Cannot {CommonDisplayTextFor.Delete.ToLower()} {DisplayTextFor.App} {app.Name} because it has one or more {Shared.Tickets.Constants.DisplayTextFor.Tickets}.");
        //}

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
