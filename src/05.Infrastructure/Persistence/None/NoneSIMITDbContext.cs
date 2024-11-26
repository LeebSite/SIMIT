using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Persistence.None;

public class NoneSIMITDbContext : DbContext, ISIMITDbContext
{
    private readonly ILogger<NoneSIMITDbContext> _logger;

    public NoneSIMITDbContext(ILogger<NoneSIMITDbContext> logger)
    {
        _logger = logger;
    }

    #region Essential Entities
    public DbSet<Audit> Audits => Set<Audit>();
    #endregion Essential Entities

    #region Business Entities
    public DbSet<Mahasiswa> Mahasiswas => Set<Mahasiswa>();
    public DbSet<Pembimbing> Pembimbings => Set<Pembimbing>();
    public DbSet<Logbook> Logbooks => Set<Logbook>();
    public DbSet<Laporan> Laporans => Set<Laporan>();

    public DbSet<LogbookAttachment> LogbookAttachments => Set<LogbookAttachment>();
    #endregion Business Entities

    private void LogWarning()
    {
        _logger.LogWarning("{ServiceName} is not implemented.", $"{nameof(Persistence)} {CommonDisplayTextFor.Service}");
    }

    public Task<int> SaveChangesAsync<THandler>(THandler handler, CancellationToken cancellationToken) where THandler : notnull
    {
        LogWarning();

        return Task.FromResult(0);
    }
}
