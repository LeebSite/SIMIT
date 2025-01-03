using System.Globalization;
using CsvHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Attributes;
using Pertamina.SIMIT.Application.Services.DateAndTime;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Shared.Audits.Queries.ExportAudits;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Application.Audits.Queries.ExportAudits;

[Authorize]
public class ExportAuditsQuery : ExportAuditsRequest, IRequest<ExportAuditsResponse>
{
}

public class ExportAuditsQueryHandler : IRequestHandler<ExportAuditsQuery, ExportAuditsResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IDateAndTimeService _dateTime;

    public ExportAuditsQueryHandler(ISIMITDbContext context, IDateAndTimeService dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<ExportAuditsResponse> Handle(ExportAuditsQuery request, CancellationToken cancellationToken)
    {
        var audits = await _context.Audits
                .Where(x => request.AuditIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
        csvWriter.WriteRecords(audits);

        var content = memoryStream.ToArray();

        return new ExportAuditsResponse
        {
            ContentType = ContentTypes.TextCsv,
            Content = content,
            FileName = $"Audits_{audits.Count}_{_dateTime.Now:yyyyMMdd_HHmmss}.csv"
        };
    }
}
