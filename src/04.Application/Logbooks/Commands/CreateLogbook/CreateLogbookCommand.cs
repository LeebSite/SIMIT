using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Logbooks.Commands.CreateLogbook;

namespace Pertamina.SIMIT.Application.Logbooks.Commands.CreateLogbook;
public class CreateLogbookCommand : CreateLogbookRequest, IRequest<CreateLogbookResponse>
{
}

public class CreateLogbookCommandValidator : AbstractValidator<CreateLogbookCommand>
{
    public CreateLogbookCommandValidator()
    {
        Include(new CreateLogbookRequestValidator());
    }
}

public class CreateLogbookCommandHandler : IRequestHandler<CreateLogbookCommand, CreateLogbookResponse>
{
    private readonly ISIMITDbContext _context;

    public CreateLogbookCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }
    public async Task<CreateLogbookResponse> Handle(CreateLogbookCommand request, CancellationToken cancellationToken)
    {

        var mahasiswa = await _context.Mahasiswas
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Nim == request.MahasiswaNim, cancellationToken);

        if (mahasiswa == null)
        {
            throw new NotFoundException($"mahasiswa with Nim '{request.MahasiswaNim}' was not found.");
        }

        var logbookExists = await _context.Logbooks
           .AsNoTracking()
           .AnyAsync(l => l.MahasiswaId == mahasiswa.Id && l.LogbookDate.Date == DateTime.UtcNow.Date, cancellationToken);

        var logbook = new Logbook
        {
            Id = Guid.NewGuid(),
            Aktifitas = request.Aktifitas,
            LogbookDate = DateTime.UtcNow.Date,
            MahasiswaId = mahasiswa.Id // Set the foreign key
        };

        _context.Logbooks.Add(logbook);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new CreateLogbookResponse
        {
            LogbookId = logbook.Id,
        };
    }
}
