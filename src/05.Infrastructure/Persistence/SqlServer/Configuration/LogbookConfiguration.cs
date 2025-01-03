
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Base.ValueObjects;
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

        builder.OwnsOne(o => o.FromGeolocation, p =>
        {
            p.Property(p => p.Latitude).HasColumnName(nameof(Geolocation.Latitude));
            p.Property(p => p.Longitude).HasColumnName(nameof(Geolocation.Longitude));
            p.Property(p => p.Accuracy).HasColumnName(nameof(Geolocation.Accuracy));
        });
    }

}
