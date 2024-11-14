using System.Transactions;
using Hangfire;
using Hangfire.MySql;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Infrastructure.BackgroundJob.Hangfire.Storages.MySql;

public static class DependencyInjection
{
    public static IServiceCollection AddHangfireUsingMySqlDatabase(this IServiceCollection services, MySqlOptions mySqlOptions, IHealthChecksBuilder healthChecksBuilder)
    {
        var mySqlStorage = new MySqlStorage(mySqlOptions.ConnectionString, new MySqlStorageOptions
        {
            TransactionIsolationLevel = IsolationLevel.ReadCommitted,
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 50000,
            TransactionTimeout = TimeSpan.FromMinutes(1),
            TablesPrefix = nameof(Hangfire)
        });

        JobStorage.Current = mySqlStorage;

        services.AddHangfire(connection =>
        {
            connection.UseSerilogLogProvider();
            connection.UseStorage(mySqlStorage);
            connection.UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        });

        healthChecksBuilder.AddMySql(
            connectionString: mySqlOptions.ConnectionString,
            name: $"{nameof(Hangfire)} {nameof(BackgroundJob).SplitWords()} {nameof(HangfireBackgroundJobOptions.Storage)} ({nameof(MySql)})");

        return services;
    }
}
