
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.UpdateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;

namespace Pertamina.SIMIT.Application.Mahasiswas.Commands.UpdateMahasiswa;
public class UpdateMahasiswaCommand : UpdateMahasiswaRequest, IRequest
{
}

public class UpdateMahasiswaCommandValidator : AbstractValidator<UpdateMahasiswaCommand>
{
    public UpdateMahasiswaCommandValidator()
    {
        Include(new UpdateMahasiswaRequestValidator());
    }
}

public class UpdateMahasiswaCommandHandler : IRequestHandler<UpdateMahasiswaCommand>
{
    private readonly ISIMITDbContext _context;

    public UpdateMahasiswaCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateMahasiswaCommand request, CancellationToken cancellationToken)
    {
        var pembimbing = await _context.Pembimbings
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == request.PembimbingId, cancellationToken);

        var mahasiswa = await _context.Mahasiswas
            .Where(x => !x.IsDeleted && x.Id == request.MahasiswaId)
            .SingleOrDefaultAsync(cancellationToken);

        if (mahasiswa is null)
        {
            throw new NotFoundException(DisplayTextFor.Mahasiswa, request.MahasiswaId);
        }

        var mahasiswaWithTheSameNim = await _context.Mahasiswas
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.Id != request.MahasiswaId && x.Nim == request.Nim)
            .SingleOrDefaultAsync(cancellationToken);

        if (mahasiswaWithTheSameNim is not null)
        {
            throw new AlreadyExistsExceptions(DisplayTextFor.Mahasiswa, DisplayTextFor.Nim, request.Nim);
        }

        mahasiswa.Nama = request.Nama;
        mahasiswa.Nim = request.Nim;
        mahasiswa.Kampus = request.Kampus;
        mahasiswa.MulaiMagang = (DateTime)request.MulaiMagang;
        mahasiswa.SelesaiMagang = (DateTime)request.SelesaiMagang;
        mahasiswa.Bagian = request.Bagian;
        mahasiswa.PembimbingId = pembimbing.Id;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}

