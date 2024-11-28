
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;

namespace Pertamina.SIMIT.Application.Mahasiswas.Commands.CreateMahasiswa;
public class CreateMahasiswaCommand : CreateMahasiswaRequest, IRequest<CreateMahasiswaResponse>
{

}

public class CreateMahasiswaCommandValidator : AbstractValidator<CreateMahasiswaCommand>
{
    public CreateMahasiswaCommandValidator()
    {
        Include(new CreateMahasiswaRequestValidator());
    }
}

public class CreateMahasiswaCommandHandler : IRequestHandler<CreateMahasiswaCommand, CreateMahasiswaResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IStorageService _storageService;

    public CreateMahasiswaCommandHandler(ISIMITDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<CreateMahasiswaResponse> Handle(CreateMahasiswaCommand request, CancellationToken cancellationToken)
    {
        var mahasiswaWithTheSameNim = await _context.Mahasiswas
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.Nim == request.Nim)
            .SingleOrDefaultAsync(cancellationToken);

        var pembimbing = await _context.Pembimbings
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == request.PembimbingId, cancellationToken);

        //var mahasiswaAttachmentWithTheSameFileName = mahasiswaWithTheSameNim.Attachments
        //    .Where(x => x.FileName == request.File.FileName)
        //    .SingleOrDefault();

        //if (mahasiswaAttachmentWithTheSameFileName is not null)
        //{
        //    throw new AlreadyExistsExceptions(Shared.MahasiswaAttachments.Constants.DisplayTextFor.Attachment, CommonDisplayTextFor.FileName, request.File.FileName);
        //}

        if (pembimbing == null)
        {
            throw new NotFoundException($"Pembimbing with Nip '{request.PembimbingId}' was not found.");
        }

        if (mahasiswaWithTheSameNim is not null)
        {
            throw new AlreadyExistsExceptions(DisplayTextFor.Mahasiswa, DisplayTextFor.Nama, request.Nama);
        }

        var mahasiswa = new Mahasiswa
        {
            Id = Guid.NewGuid(),
            Nama = request.Nama,
            Nim = request.Nim,
            Kampus = request.Kampus,
            MulaiMagang = (DateTime)request.MulaiMagang,
            SelesaiMagang = (DateTime)request.SelesaiMagang,
            Bagian = request.Bagian,
            PembimbingId = pembimbing.Id, // Set the foreign key
        };

        _context.Mahasiswas.Add(mahasiswa);

        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        var file = memoryStream.ToArray();

        var mahasiswaAttachment = new MahasiswaAttachment
        {
            Id = Guid.NewGuid(),
            MahasiswaId = mahasiswa.Id,
            FileName = request.File.FileName,
            FileSize = request.File.Length,
            FileContentType = request.File.ContentType,
            StorageFileId = await _storageService.CreateAsync(file)
        };

        //mahasiswaAttachment.DomainEvents.Add(new MahasiswaAttachmentCreatedEvent(mahasiswaAttachment.Id))
        await _context.MahasiswaAttachments.AddAsync(mahasiswaAttachment, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        try
        {
            await _context.SaveChangesAsync(this, cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error: {ex.InnerException?.Message}");
            throw;
        }

        return new CreateMahasiswaResponse
        {
            MahasiswaId = mahasiswa.Id,
        };
    }
}
