using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbings;

namespace Pertamina.SIMIT.Application.Pembimbings.Commands.UpdatePembimbings;
public class UpdatePembimbingsCommand : UpdatePembimbingsRequest, IRequest<UpdatePembimbingsResponse>
{
}
public class UpdatePembimbingsCommandHandler : IRequestHandler<UpdatePembimbingsCommand, UpdatePembimbingsResponse>
{
    private readonly ISIMITDbContext _context;

    public UpdatePembimbingsCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<UpdatePembimbingsResponse> Handle(UpdatePembimbingsCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdatePembimbingsResponse();

        foreach (var updatedPembimbing in request.Pembimbings)
        {
            var pembimbing = await _context.Pembimbings
                .Where(x => x.Id == updatedPembimbing.PembimbingId)
                .SingleOrDefaultAsync(cancellationToken);

            if (pembimbing is null)
            {
                continue;
            }

            var pembimbingWithTheSameName = await _context.Pembimbings
                .AsNoTracking()
                .Where(x => x.Id != updatedPembimbing.PembimbingId && x.Nama == updatedPembimbing.Nama)
                .SingleOrDefaultAsync(cancellationToken);

            if (pembimbingWithTheSameName is not null)
            {
                continue;
            }

            pembimbing.Nama = updatedPembimbing.Nama;
            pembimbing.Nip = updatedPembimbing.Nip;
            pembimbing.Jabatan = updatedPembimbing.Jabatan;
            pembimbing.Email = updatedPembimbing.Email;

            response.PembimbingsUpdated++;
        }

        if (response.PembimbingsUpdated > 0)
        {
            await _context.SaveChangesAsync(this, cancellationToken);
        }

        return response;
    }
}

