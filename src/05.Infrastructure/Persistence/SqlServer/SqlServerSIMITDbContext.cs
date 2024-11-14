using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Services.CurrentUser;
using Pertamina.SIMIT.Application.Services.DateAndTime;
using Pertamina.SIMIT.Application.Services.DomainEvent;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Extensions;
using Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Configuration;

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer;

public class SqlServerSIMITDbContext : SIMITDbContext
{
    public SqlServerSIMITDbContext(
        DbContextOptions<SqlServerSIMITDbContext> options,
        ICurrentUserService currentUser,
        IDateAndTimeService dateTime,
        IDomainEventService domainEvent) : base(options)
    {
        _currentUser = currentUser;
        _dateTime = dateTime;
        _domainEvent = domainEvent;
    }

    public SqlServerSIMITDbContext(DbContextOptions<SqlServerSIMITDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromNameSpace(typeof(AuditConfiguration).Namespace!);

        base.OnModelCreating(builder);
    }
}
