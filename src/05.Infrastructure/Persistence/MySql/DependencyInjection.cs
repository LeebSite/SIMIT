using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Infrastructure.Persistence.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Persistence.MySql;

public static class DependencyInjection
{
    public static IServiceCollection AddMySqlPersistenceService(this IServiceCollection services, MySqlOptions mySqlOptions, IHealthChecksBuilder healthChecksBuilder)
    {
        var migrationsAssembly = typeof(MySqlSIMITDbContext).Assembly.FullName;

        services.AddDbContext<MySqlSIMITDbContext>(options =>
        {
            options.UseMySql(mySqlOptions.ConnectionString, ServerVersion.AutoDetect(mySqlOptions.ConnectionString), builder =>
            {
                builder.MigrationsAssembly(migrationsAssembly);
                builder.MigrationsHistoryTable(TableNameFor.EfMigrationsHistory);
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            options.ConfigureWarnings(wcb => wcb.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
            options.ConfigureWarnings(wcb => wcb.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        });

        services.AddScoped<ISIMITDbContext>(provider => provider.GetRequiredService<MySqlSIMITDbContext>());

        healthChecksBuilder.AddMySql(
            connectionString: mySqlOptions.ConnectionString,
            name: $"{nameof(Persistence)} {nameof(PersistenceOptions.Provider)} ({nameof(MySql)})");

        return services;
    }
}
