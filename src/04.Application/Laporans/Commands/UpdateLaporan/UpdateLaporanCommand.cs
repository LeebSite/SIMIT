using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Laporans.Commands.UpdateLaporan;
using Pertamina.SIMIT.Shared.Laporans.Constants;

namespace Pertamina.SIMIT.Application.Laporans.Commands.UpdateLaporan;
public class UpdateLaporanCommand : UpdateLaporanRequest, IRequest
{
}

public class UpdateLaporanCommandValidator : AbstractValidator<UpdateLaporanCommand>
{
    public UpdateLaporanCommandValidator()
    {
        Include(new UpdateLaporanRequestValidator());
    }
}

public class UpdateLaporanCommandHandler : IRequestHandler<UpdateLaporanCommand>
{
    private readonly ISIMITDbContext _context;

    public UpdateLaporanCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLaporanCommand request, CancellationToken cancellationToken)
    {
        var laporan = await _context.Laporans
            .Where(x => !x.IsDeleted && x.Id == request.LaporanId)
            .Include(a => a.MahasiswaId)
            .SingleOrDefaultAsync(cancellationToken);

        if (laporan is null)
        {
            throw new NotFoundException(DisplayTextFor.Laporan, request.LaporanId);
        }

        laporan.MahasiswaId = request.MahasiswaId;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}

