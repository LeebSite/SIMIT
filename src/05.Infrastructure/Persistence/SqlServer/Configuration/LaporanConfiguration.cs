
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Constants;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Extensions;
using Pertamina.SIMIT.Shared.Laporans.Constants;

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Configuration;
public class LaporanConfiguration : IEntityTypeConfiguration<Laporan>
{
    public void Configure(EntityTypeBuilder<Laporan> builder)
    {
        builder.ToTable(nameof(ISIMITDbContext.Laporans), nameof(SIMIT));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.Property(e => e.Deskripsi).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Deskripsi));
        builder.Property(e => e.FileLaporan).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.FileLaporan));
        builder.Property(e => e.FileProject).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.FileProject));
    }
}
