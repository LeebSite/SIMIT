using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswas;

namespace Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswas;
public class GetMahasiswaCount : IRequest<GetMahasiswasMahasiswa>
{
}

public class GetMahasiswaCountHandler : IRequestHandler<GetMahasiswaCount, GetMahasiswasMahasiswa>
{
    private readonly ISIMITDbContext _context;

    public GetMahasiswaCountHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<GetMahasiswasMahasiswa> Handle(GetMahasiswaCount request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // Query jumlah mahasiswa aktif
        var activeMahasiswaCount = await _context.Mahasiswas
            .AsNoTracking()
            .Where(m => !m.IsDeleted && m.SelesaiMagang > now)
            .CountAsync(cancellationToken);

        // Query jumlah laporan yang sudah dikumpulkan
        var laporanSubmittedCount = await _context.Laporans
            .AsNoTracking()
            .Where(l => !l.IsDeleted && l.Mahasiswa.SelesaiMagang > now)
            .Select(l => l.MahasiswaId)
            .Distinct()
            .CountAsync(cancellationToken);

        return new GetMahasiswasMahasiswa
        {
            Mahasiswa = activeMahasiswaCount,
            Laporan = laporanSubmittedCount
        };
    }
}

