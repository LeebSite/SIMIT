
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Constants;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Extensions;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Configuration;
public class MahasiswaConfiguration : IEntityTypeConfiguration<Mahasiswa>
{
    public void Configure(EntityTypeBuilder<Mahasiswa> builder)
    {
        builder.ToTable(nameof(ISIMITDbContext.Mahasiswas), nameof(SIMIT));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.Property(e => e.Nama).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Nama));
        builder.Property(e => e.Nim).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Nim));
        builder.Property(e => e.Kampus).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Kampus));
        builder.Property(e => e.MulaiMagang).HasColumnType("datetime");
        builder.Property(e => e.SelesaiMagang).HasColumnType("datetime");
        builder.Property(e => e.Bagian).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Bagian));
    }
}
