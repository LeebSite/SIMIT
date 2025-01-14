using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Constants;

namespace Pertamina.SIMIT.Application.Pembimbings.Commands.UpdatePembimbing;
public class UpdatePembimbingCommand : UpdatePembimbingRequest, IRequest
{

}

public class UpdatePembimbingCommandValidator : AbstractValidator<UpdatePembimbingCommand>
{
    public UpdatePembimbingCommandValidator()
    {
        Include(new UpdatePembimbingRequestValidator());
    }
}

public class UpdatePembimbingCommandHandler : IRequestHandler<UpdatePembimbingCommand>
{
    private readonly ISIMITDbContext _context;

    public UpdatePembimbingCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePembimbingCommand request, CancellationToken cancellationToken)
    {
        var pembimbing = await _context.Pembimbings
            .Where(x => x.Id == request.PembimbingId)
            .SingleOrDefaultAsync(cancellationToken);

        if (pembimbing is null)
        {
            throw new NotFoundException(DisplayTextFor.Pembimbing, request.PembimbingId);
        }

        var pembimbingWithTheSameName = await _context.Pembimbings
            .AsNoTracking()
            .Where(x => x.Id != request.PembimbingId && x.Nip == request.Nip)
            .SingleOrDefaultAsync(cancellationToken);

        if (pembimbingWithTheSameName is not null)
        {
            throw new AlreadyExistsExceptions(DisplayTextFor.Pembimbing, DisplayTextFor.Nip, request.Nip);
        }

        pembimbing.Nama = request.Nama;
        pembimbing.Nip = request.Nip;
        pembimbing.Jabatan = request.Jabatan;
        pembimbing.Email = request.Email;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
