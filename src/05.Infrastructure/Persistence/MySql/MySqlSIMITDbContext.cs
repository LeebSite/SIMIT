using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Services.CurrentUser;
using Pertamina.SIMIT.Application.Services.DateAndTime;
using Pertamina.SIMIT.Application.Services.DomainEvent;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Extensions;
using Pertamina.SIMIT.Infrastructure.Persistence.MySql.Configuration;

namespace Pertamina.SIMIT.Infrastructure.Persistence.MySql;

public class MySqlSIMITDbContext : SIMITDbContext
{
    public MySqlSIMITDbContext(
        DbContextOptions<MySqlSIMITDbContext> options,
        ICurrentUserService currentUser,
        IDateAndTimeService dateTime,
        IDomainEventService domainEvent) : base(options)
    {
        _currentUser = currentUser;
        _dateTime = dateTime;
        _domainEvent = domainEvent;
    }

    public MySqlSIMITDbContext(DbContextOptions<MySqlSIMITDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromNameSpace(typeof(AuditConfiguration).Namespace!);

        base.OnModelCreating(builder);
    }
}
