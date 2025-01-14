using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.CreatePembimbing;

namespace Pertamina.SIMIT.Application.Pembimbings.Commands.CreatePembimbing;
public class CreatePembimbingCommand : CreatePembimbingRequest, IRequest<CreatePembimbingResponse>
{

}

public class CreatePembimbingCommandValidator : AbstractValidator<CreatePembimbingCommand>
{
    public CreatePembimbingCommandValidator()
    {
        Include(new CreatePembimbingRequestValidator());
    }
}

public class CreatePembimbingCommandHandler : IRequestHandler<CreatePembimbingCommand, CreatePembimbingResponse>
{
    private readonly ISIMITDbContext _context;

    public CreatePembimbingCommandHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<CreatePembimbingResponse> Handle(CreatePembimbingCommand request, CancellationToken cancellationToken)
    {
        var pembimbingWithTheSameName = await _context.Pembimbings
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Nip == request.Nip, cancellationToken);

        if (pembimbingWithTheSameName != null)
        {
            throw new InvalidOperationException("Pembimbing dengan nama atau NIP yang sama sudah terdaftar.");
        }

        var pembimbing = new Pembimbing
        {
            Id = Guid.NewGuid(),
            Nama = request.Nama,
            Nip = request.Nip,
            Jabatan = request.Jabatan,
            Email = request.Email,
        };

        _context.Pembimbings.Add(pembimbing);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new CreatePembimbingResponse
        {
            PembimbingId = pembimbing.Id
        };
    }
}
