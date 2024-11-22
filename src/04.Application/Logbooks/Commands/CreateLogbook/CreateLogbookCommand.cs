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
        //var mahasiswaWithTheSameNim = await _context.Mahasiswas
        //    .AsNoTracking()
        //    //.Where(x => !x.IsDeleted && x.Nim == request.Nim)
        //    .SingleOrDefaultAsync(cancellationToken);

        var mahasiswa = await _context.Mahasiswas
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == request.MahasiswaId, cancellationToken);

        if (mahasiswa == null)
        {
            throw new NotFoundException($"Logbook with Nim '{request.MahasiswaId}' was not found.");
        }

        //if (mahasiswaWithTheSameNim is not null)
        //{
        //    throw new AlreadyExistsExceptions(DisplayTextFor.Mahasiswa, DisplayTextFor.Nama, request.Nama);
        //}

        var logbook = new Logbook
        {
            Id = Guid.NewGuid(),
            LogbookDate = (DateTime)request.LogbookDate,
            Aktifitas = request.Aktifitas,
            MahasiswaId = mahasiswa.Id, // Set the foreign key
        };

        _context.Logbooks.Add(logbook);
        try
        {
            await _context.SaveChangesAsync(this, cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error: {ex.InnerException?.Message}");
            throw;
        }

        return new CreateLogbookResponse
        {
            LogbookId = logbook.Id,
        };
    }
}
