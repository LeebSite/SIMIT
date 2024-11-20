using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.UpdateMahasiswas;

namespace Pertamina.SIMIT.Application.Mahasiswas.Commands.UpdateMahasiswas;

public class UpdateMahasiswasCommand : UpdateMahasiswasRequest, IRequest<UpdateMahasiswasResponse>
{
}

public class UpdateAppsCommandHandler : IRequestHandler<UpdateMahasiswasCommand, UpdateMahasiswasResponse>
{
    private readonly ISIMITDbContext _context;

    public UpdateAppsCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateMahasiswasResponse> Handle(UpdateMahasiswasCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdateMahasiswasResponse();

        foreach (var updatedMahasiswa in request.Mahasiswas)
        {
            var mahasiswa = await _context.Mahasiswas
                .Where(x => !x.IsDeleted && x.Id == updatedMahasiswa.MahasiswaId)
                .SingleOrDefaultAsync(cancellationToken);

            if (mahasiswa is null)
            {
                continue;
            }

            var mahasiswaWithTheSameNim = await _context.Mahasiswas
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Id != updatedMahasiswa.MahasiswaId && x.Nim == updatedMahasiswa.Nim)
                .SingleOrDefaultAsync(cancellationToken);

            if (mahasiswaWithTheSameNim is not null)
            {
                continue;
            }

            mahasiswa.Nama = updatedMahasiswa.Nama;
            mahasiswa.Nim = updatedMahasiswa.Nim;
            mahasiswa.Kampus = updatedMahasiswa.Kampus;
            mahasiswa.MulaiMagang = (DateTime)updatedMahasiswa.MulaiMagang;
            mahasiswa.SelesaiMagang = (DateTime)updatedMahasiswa.SelesaiMagang;
            mahasiswa.Bagian = updatedMahasiswa.Bagian;
            mahasiswa.PembimbingId = updatedMahasiswa.PembimbingId;

            response.MahasiswasUpdated++;
        }

        if (response.MahasiswasUpdated > 0)
        {
            await _context.SaveChangesAsync(this, cancellationToken);
        }

        return response;
    }
}

