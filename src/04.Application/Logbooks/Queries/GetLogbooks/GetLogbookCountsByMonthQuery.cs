using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Services.Persistence;

namespace Pertamina.SIMIT.Application.Logbooks.Queries.GetLogbooks;
public class GetLogbooksPerMonthQuery : IRequest<Dictionary<string, int>>
{
}

public class GetLogbooksPerMonthHandler : IRequestHandler<GetLogbooksPerMonthQuery, Dictionary<string, int>>
{
    private readonly ISIMITDbContext _context;

    public GetLogbooksPerMonthHandler(ISIMITDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, int>> Handle(GetLogbooksPerMonthQuery request, CancellationToken cancellationToken)
    {
        var currentYear = DateTime.UtcNow.Year;

        var logbooksPerMonth = await _context.Logbooks
            .AsNoTracking()
            .Where(l => l.LogbookDate.Year == currentYear)
            .GroupBy(l => l.LogbookDate.Month)
            .Select(g => new { Month = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var result = new Dictionary<string, int>();
        for (var i = 1; i <= 12; i++)
        {
            var monthName = new DateTime(currentYear, i, 1).ToString("MMMM");
            result[monthName] = logbooksPerMonth.FirstOrDefault(m => m.Month == i)?.Count ?? 0;
        }

        // Tangani null dengan data default jika logbooks kosong
        if (!result.Any())
        {
            result["No Data"] = 0;
        }

        return result;
    }

}
