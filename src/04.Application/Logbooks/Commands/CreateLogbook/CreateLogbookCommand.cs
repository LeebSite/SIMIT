﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Base.ValueObjects;
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
    private readonly IStorageService _storageService;

    public CreateLogbookCommandHandler(ISIMITDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<CreateLogbookResponse> Handle(CreateLogbookCommand request, CancellationToken cancellationToken)
    {

        var mahasiswa = await _context.Mahasiswas
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Nim == request.MahasiswaNim, cancellationToken);

        if (mahasiswa == null)
        {
            throw new NotFoundException($"Logbook with Nim '{request.MahasiswaNim}' was not found.");
        }

        // Menentukan apakah logbook ini untuk sesi pagi atau siang
        var logbookDate = (DateTime)request.LogbookDate;
        var isMorningSession = logbookDate.Hour is >= 7 and <= 12;
        var isAfternoonSession = logbookDate.Hour is >= 13 and <= 16;

        // Memeriksa apakah mahasiswa sudah memiliki logbook untuk sesi yang sama pada hari yang sama
        var existingLogbook = await _context.Logbooks
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.MahasiswaId == mahasiswa.Id && l.LogbookDate.Date == logbookDate.Date
                                      && ((isMorningSession && l.LogbookDate.Hour >= 7 && l.LogbookDate.Hour <= 12)
                                      || (isAfternoonSession && l.LogbookDate.Hour >= 13 && l.LogbookDate.Hour <= 16)), cancellationToken);

        if (existingLogbook != null)
        {
            throw new InvalidOperationException($"Mahasiswa dengan No Badge '{request.MahasiswaNim}' sudah menginput logbook pada sesi ini.");
        }

        var logbook = new Logbook
        {
            Id = Guid.NewGuid(),
            LogbookDate = (DateTime)request.LogbookDate,
            Aktifitas = request.Aktifitas,
            MahasiswaId = mahasiswa.Id, // Set the foreign key
            FromGeolocation = new Geolocation(request.Latitude, request.Longitude, request.Accuracy) // Data geolocation
        };

        _context.Logbooks.Add(logbook);

        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        var file = memoryStream.ToArray();

        var logbookAttachment = new LogbookAttachment
        {
            Id = Guid.NewGuid(),
            LogbookId = logbook.Id,
            FileName = request.File.Name,
            FileSize = request.File.Length,
            FileContentType = request.File.ContentType,
            StorageFileId = await _storageService.CreateAsync(file)
        };

        await _context.LogbookAttachments.AddAsync(logbookAttachment, cancellationToken);
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

        return new CreateLogbookResponse
        {
            LogbookId = logbook.Id,
        };
    }
}
