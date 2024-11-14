using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Pertamina.SIMIT.Application;
using Pertamina.SIMIT.Infrastructure;
using Pertamina.SIMIT.Infrastructure.Logging;
using Pertamina.SIMIT.Infrastructure.Persistence;
using Pertamina.SIMIT.Shared;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.WebApi.Common.Filters.ApiException;
using Pertamina.SIMIT.WebApi.Common.ModelBindings;
using Pertamina.SIMIT.WebApi.Services;
using Pertamina.SIMIT.WebApi.Services.BackEnd;

Console.WriteLine($"Starting {CommonValueFor.EntryAssemblySimpleName}...");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLoggingService();
builder.Services.AddShared(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebApi(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddApiVersioning();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ApiExceptionFilterAttribute());
    options.ModelBinderProviders.Insert(0, new JsonModelBinderProvider());
})
.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

using var scope = app.Services.CreateScope();
await scope.ServiceProvider.ApplyDatabaseMigrationAsync<Program>();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Running {AssemblyName}", CommonValueFor.EntryAssemblySimpleName);

var backEndOptions = builder.Configuration.GetSection(BackEndOptions.SectionKey).Get<BackEndOptions>();

if (!string.IsNullOrWhiteSpace(backEndOptions.BasePath))
{
    app.UsePathBase(backEndOptions.BasePath);
}

if (app.Environment.IsEnvironment(EnvironmentNames.Local) || app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseInfrastructure(builder.Configuration);
app.UseWebApi(builder.Configuration);
app.MapControllers();
app.Run();
