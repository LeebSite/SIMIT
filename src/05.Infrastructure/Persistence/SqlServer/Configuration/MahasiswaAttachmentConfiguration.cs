using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Extensions;

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Configuration;

public class MahasiswaAttachmentConfiguration : IEntityTypeConfiguration<MahasiswaAttachment>
{
    public void Configure(EntityTypeBuilder<MahasiswaAttachment> builder)
    {
        builder.ToTable(nameof(ISIMITDbContext.MahasiswaAttachments), nameof(SIMIT));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();
        builder.ConfigureFileProperties();

        builder.HasOne(e => e.Mahasiswa).WithMany(e => e.Attachments).HasForeignKey(e => e.MahasiswaId).OnDelete(DeleteBehavior.Restrict);
    }
}
