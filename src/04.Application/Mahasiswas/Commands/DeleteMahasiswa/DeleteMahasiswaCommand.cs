
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;

namespace Pertamina.SIMIT.Application.Mahasiswas.Commands.DeleteMahasiswa;
public class DeleteMahasiswaCommand : IRequest
{
    public Guid MahasiswaId { get; set; }
}

public class DeleteMahasiswaCommandHandler : IRequestHandler<DeleteMahasiswaCommand>
{
    private readonly ISIMITDbContext _context;

    public DeleteMahasiswaCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMahasiswaCommand request, CancellationToken cancellationToken)
    {
        var mahasiswa = await _context.Mahasiswas
            .Where(x => !x.IsDeleted && x.Id == request.MahasiswaId)
            .SingleOrDefaultAsync(cancellationToken);

        if (mahasiswa is null)
        {
            throw new NotFoundException(DisplayTextFor.Mahasiswa, request.MahasiswaId);
        }

        mahasiswa.IsDeleted = true;
        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
