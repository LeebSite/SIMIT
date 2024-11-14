using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pertamina.SIMIT.Infrastructure.Email.FluentMailkit;

public static class CustomFluentEmailMailKitBuilderExtensions
{
    public static FluentEmailServicesBuilder AddFluentMailKitSender(this FluentEmailServicesBuilder builder, FluentSmtpClientOptions fluentSmtpClientOptions)
    {
        builder.Services.TryAdd(ServiceDescriptor.Scoped<ISender>(_ => new FluentMailKitSender(fluentSmtpClientOptions)));
        return builder;
    }
}
