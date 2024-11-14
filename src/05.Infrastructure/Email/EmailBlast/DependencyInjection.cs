using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Email.EmailBlast;

public static class DependencyInjection
{
    public static FluentEmailServicesBuilder AddEmailBlastEmailService(this FluentEmailServicesBuilder fluentEmailServicesBuilder, EmailBlastEmailOptions emailBlastEmailOptions, IHealthChecksBuilder healthChecksBuilder)
    {
        fluentEmailServicesBuilder.Services.TryAdd(ServiceDescriptor.Scoped<ISender>(_ => new EmailBlastEmailSender(emailBlastEmailOptions)));

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Email)} {CommonDisplayTextFor.Service} ({nameof(EmailBlast)})",
            instance: new EmailBlastEmailHealthCheck(emailBlastEmailOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Degraded,
            tags: default));

        return fluentEmailServicesBuilder;
    }
}
