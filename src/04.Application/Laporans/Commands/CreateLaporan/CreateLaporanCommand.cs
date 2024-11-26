using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Laporans.Commands.CreateLaporan;
using Pertamina.SIMIT.Shared.Laporans.Constants;

namespace Pertamina.SIMIT.Application.Laporans.Commands.CreateLaporan;
public class CreateLaporanCommand : CreateLaporanRequest, IRequest<CreateLaporanResponse>
{
}

public class CreateLaporanCommandValidator : AbstractValidator<CreateLaporanCommand>
{
    public CreateLaporanCommandValidator()
    {
        // Use CreateLaporanRequestValidator instead of including itself
        Include(new CreateLaporanRequestValidator());
    }
}

public class CreateLaporanCommandHandler : IRequestHandler<CreateLaporanCommand, CreateLaporanResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IStorageService _storageService;

    public CreateLaporanCommandHandler(ISIMITDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<CreateLaporanResponse> Handle(CreateLaporanCommand request, CancellationToken cancellationToken)
    {
        // Validasi NIM Mahasiswa
        var mahasiswa = await _context.Mahasiswas
            .AsNoTracking()
            .Where(x => x.Nim == request.MahasiswaNim && !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        if (mahasiswa is null)
        {
            throw new NotFoundException($"{DisplayTextFor.Mahasiswa} dengan NIM {request.MahasiswaNim}", request.MahasiswaNim);
        }

        // File handling
        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        var file = memoryStream.ToArray();

        var laporan = new Laporan
        {
            Id = Guid.NewGuid(),
            MahasiswaId = mahasiswa.Id,
            FileName = request.File.FileName,
            FileSize = request.File.Length,
            FileContentType = request.File.ContentType,
            StorageFileId = await _storageService.CreateAsync(file),
            Deskripsi = request.Deskripsi
        };

        await _context.Laporans.AddAsync(laporan, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new CreateLaporanResponse
        {
            LaporanId = laporan.Id
        };
    }

}
