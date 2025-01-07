
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Constants;
using Pertamina.SIMIT.Shared.Pembimbings.Constants;

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Configuration;
public class PembimbingConfiguration : IEntityTypeConfiguration<Pembimbing>
{
    public void Configure(EntityTypeBuilder<Pembimbing> builder)
    {
        builder.ToTable(nameof(ISIMITDbContext.Pembimbings), nameof(SIMIT));

        builder.Property(e => e.Nama).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Nama));
        builder.Property(e => e.Nip).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Nip));
        builder.Property(e => e.Jabatan).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Jabatan));
        builder.Property(e => e.Email).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Email));
    }
}
