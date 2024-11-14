using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pertamina.SIMIT.Infrastructure.Email.FluentMailkit;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Email.Smtp;

public static class DependencyInjection
{
    public static FluentEmailServicesBuilder AddSmtpEmailService(this FluentEmailServicesBuilder fluentEmailServicesBuilder, SmtpEmailOptions smtpEmailOptions, IHealthChecksBuilder healthChecksBuilder)
    {
        var fluentSmtpClientOptions = new FluentSmtpClientOptions
        {
            Server = smtpEmailOptions.Host,
            Port = smtpEmailOptions.Port,
            User = smtpEmailOptions.Username,
            Password = smtpEmailOptions.Password,
            UseSsl = smtpEmailOptions.EnableSsl,
            SocketOptions = MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable,
            RequiresAuthentication = true
        };

        fluentEmailServicesBuilder.AddFluentMailKitSender(fluentSmtpClientOptions);

        healthChecksBuilder.AddSmtpHealthCheck(
            setup: options =>
            {
                options.Host = smtpEmailOptions.Host;
                options.Port = smtpEmailOptions.Port;
                options.AllowInvalidRemoteCertificates = true;
            },
            name: $"{nameof(Email)} {CommonDisplayTextFor.Service} ({nameof(Smtp)})",
            failureStatus: HealthStatus.Degraded);

        return fluentEmailServicesBuilder;
    }
}
