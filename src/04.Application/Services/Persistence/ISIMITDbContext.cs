using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Domain.Entities;

namespace Pertamina.SIMIT.Application.Services.Persistence;

public interface ISIMITDbContext
{
    #region Essential Entities
    DbSet<Audit> Audits { get; }
    #endregion Essential Entities

    #region Business Entities
    DbSet<Laporan> Laporans { get; }
    DbSet<Mahasiswa> Mahasiswas { get; }
    DbSet<Logbook> Logbooks { get; }
    DbSet<Pembimbing> Pembimbings { get; }
    DbSet<MahasiswaAttachment> MahasiswaAttachments { get; }
    #endregion Business Entities

    Task<int> SaveChangesAsync<THandler>(THandler handler, CancellationToken cancellationToken = default) where THandler : notnull;
}
