using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
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
            throw new NotFoundException($"Mahasiswa dengan NIM {request.MahasiswaNim} tidak ditemukan.");
        }

        // Periksa apakah laporan untuk MahasiswaId sudah ada
        var laporan = await _context.Laporans
            .Where(x => x.MahasiswaId == mahasiswa.Id && !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        // File handling
        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
        var fileContent = memoryStream.ToArray();

        if (laporan is not null)
        {
            // Update laporan jika sudah ada
            laporan.FileName = request.File.FileName;
            laporan.FileSize = request.File.Length;
            laporan.FileContentType = request.File.ContentType;
            laporan.Deskripsi = request.Deskripsi;

            // Simpan file baru ke storage
            laporan.StorageFileId = await _storageService.CreateAsync(fileContent);

            // Tandai laporan sebagai telah dimodifikasi
            laporan.Modified = DateTimeOffset.UtcNow;
            laporan.ModifiedBy = "System"; // Sesuaikan dengan pengguna
        }
        else
        {
            // Buat laporan baru jika belum ada
            laporan = new Laporan
            {
                Id = Guid.NewGuid(),
                MahasiswaId = mahasiswa.Id,
                FileName = request.File.FileName,
                FileSize = request.File.Length,
                FileContentType = request.File.ContentType,
                Deskripsi = request.Deskripsi,
                StorageFileId = await _storageService.CreateAsync(fileContent),
                Created = DateTimeOffset.UtcNow,
                CreatedBy = "System" // Sesuaikan dengan pengguna
            };

            await _context.Laporans.AddAsync(laporan, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateLaporanResponse
        {
            LaporanId = laporan.Id
        };
    }

}
