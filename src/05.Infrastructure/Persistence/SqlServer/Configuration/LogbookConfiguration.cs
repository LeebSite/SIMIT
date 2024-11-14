
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Constants;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Extensions;
using Pertamina.SIMIT.Shared.Logbooks.Constants;

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Configuration;
public class LogbookConfiguration : IEntityTypeConfiguration<Logbook>
{
    public void Configure(EntityTypeBuilder<Logbook> builder)
    {
        builder.ToTable(nameof(ISIMITDbContext.Logbooks), nameof(SIMIT));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.Property(e => e.Aktifitas).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Aktifitas));
    }
}
