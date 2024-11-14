using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Infrastructure.AppInfo;
using Pertamina.SIMIT.Infrastructure.Authentication;
using Pertamina.SIMIT.Infrastructure.Authorization;
using Pertamina.SIMIT.Infrastructure.BackgroundJob;
using Pertamina.SIMIT.Infrastructure.CurrentUser;
using Pertamina.SIMIT.Infrastructure.DateAndTime;
using Pertamina.SIMIT.Infrastructure.DomainEvent;
using Pertamina.SIMIT.Infrastructure.Ecm;
using Pertamina.SIMIT.Infrastructure.Email;
using Pertamina.SIMIT.Infrastructure.HealthCheck;
using Pertamina.SIMIT.Infrastructure.Otp;
using Pertamina.SIMIT.Infrastructure.Persistence;
using Pertamina.SIMIT.Infrastructure.Sms;
using Pertamina.SIMIT.Infrastructure.Storage;
using Pertamina.SIMIT.Infrastructure.Telemetry;
using Pertamina.SIMIT.Infrastructure.UserProfile;

namespace Pertamina.SIMIT.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Health Check
        var healthChecksBuilder = services.AddHealthCheckService(configuration);
        #endregion Health Check

        #region AppInfo
        services.AddAppInfoService(configuration);
        #endregion AppInfo

        #region Authentication
        services.AddAuthenticationService(configuration, healthChecksBuilder);
        #endregion Authentication

        #region Authorization
        services.AddAuthorizationService(configuration, healthChecksBuilder);
        #endregion Authorization

        #region Background Job
        services.AddBackgroundJobService(configuration, healthChecksBuilder);
        #endregion Background Job

        #region Current User
        services.AddCurrentUserService();
        #endregion Current User

        #region DateTime
        services.AddDateAndTimeService();
        #endregion DateTime

        #region Domain Event
        services.AddDomainEventService();
        #endregion Domain Event

        #region Enterprise Content Management
        services.AddEcmService(configuration, healthChecksBuilder);
        #endregion Enterprise Content Management

        #region Email
        services.AddEmailService(configuration, healthChecksBuilder);
        #endregion Email

        #region One Time Password
        services.AddOtpService();
        #endregion One Time Password

        #region Persistence
        services.AddPersistenceService(configuration, healthChecksBuilder);
        #endregion Persistence

        #region SMS
        services.AddSmsService(configuration, healthChecksBuilder);
        #endregion SMS

        #region Storage
        services.AddStorageService(configuration, healthChecksBuilder);
        #endregion Storage

        #region Telemetry
        services.AddTelemetryService(configuration);
        #endregion Telemetry

        #region User Profile
        services.AddUserProfileService(configuration, healthChecksBuilder);
        #endregion User Profile

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration)
    {
        #region Health Check
        app.UseHealthCheckService(configuration);
        #endregion Health Check

        #region Background Job
        app.UseBackgroundJobService(configuration);
        #endregion Background Job

        return app;
    }
}
