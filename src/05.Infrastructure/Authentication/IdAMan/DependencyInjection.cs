using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Services.Authentication.Constants;

namespace Pertamina.SIMIT.Infrastructure.Authentication.IdAMan;

public static class DependencyInjection
{
    public static IServiceCollection AddIdAManAuthenticationService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<IdAManAuthenticationOptions>(configuration.GetSection(IdAManAuthenticationOptions.SectionKey));

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var idAManAuthenticationOptions = configuration.GetSection(IdAManAuthenticationOptions.SectionKey).Get<IdAManAuthenticationOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = idAManAuthenticationOptions.AuthorityUrl;
                options.Audience = $"{PrefixFor.ApiScope}{idAManAuthenticationOptions.ObjectId}";
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var requestPath = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host.Value}{context.HttpContext.Request.Path.Value}";

                        if (context.HttpContext.Request.Query.Any())
                        {
                            requestPath += "?";

                            foreach (var item in context.HttpContext.Request.Query)
                            {
                                requestPath += $"{item.Key}={item.Value}";
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Authentication)} {CommonDisplayTextFor.Service} ({nameof(IdAMan)})",
            instance: new IdAManAuthenticationHealthCheck(idAManAuthenticationOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
