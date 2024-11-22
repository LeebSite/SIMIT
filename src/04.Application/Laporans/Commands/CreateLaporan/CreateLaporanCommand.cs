
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Laporans.Commands.CreateLaporan;

namespace Pertamina.SIMIT.Application.Laporans.Commands.CreateLaporan;
public class CreateLaporanCommand : CreateLaporanRequest, IRequest<CreateLaporanResponse>
{
}

public class CreateLaporanCommandValidator : AbstractValidator<CreateLaporanCommand>
{
    public CreateLaporanCommandValidator()
    {
        Include(new CreateLaporanRequestValidator());
    }
}

public class CreateLaporanCommandHandler : IRequestHandler<CreateLaporanCommand, CreateLaporanResponse>
{
    private readonly ISIMITDbContext _context;

    public CreateLaporanCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }
    public async Task<CreateLaporanResponse> Handle(CreateLaporanCommand request, CancellationToken cancellationToken)
    {

        var mahasiswa = await _context.Mahasiswas
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == request.MahasiswaId, cancellationToken);

        if (mahasiswa == null)
        {
            throw new NotFoundException($"mahasiswa with Nim '{request.MahasiswaId}' was not found.");
        }

        var laporanExists = await _context.Laporans
           .AsNoTracking()
           .AnyAsync(la => la.MahasiswaId == mahasiswa.Id, cancellationToken);

        var laporan = new Laporan
        {
            Id = Guid.NewGuid(),
            FileLaporan = request.FileLaporan,
            FileProject = request.FileProject,
            Deskripsi = request.Deskripsi,
            MahasiswaId = mahasiswa.Id // Set the foreign key
        };

        _context.Laporans.Add(laporan);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new CreateLaporanResponse
        {
            LaporandId = laporan.Id,
        };
    }
}
