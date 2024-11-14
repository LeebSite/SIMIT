using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Services;
using Pertamina.SIMIT.Bsui.Services.Authentication.IdAMan;
using Pertamina.SIMIT.Bsui.Services.Logging;
using Pertamina.SIMIT.Bsui.Services.Security;
using Pertamina.SIMIT.Client;
using Pertamina.SIMIT.Shared;
using Pertamina.SIMIT.Shared.Common.Constants;

Console.WriteLine($"Starting {CommonValueFor.EntryAssemblySimpleName}...");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLoggingService();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddShared(builder.Configuration);
builder.Services.AddClient(builder.Configuration);
builder.Services.AddBsui(builder.Configuration);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Running {AssemblyName}", CommonValueFor.EntryAssemblySimpleName);

if (app.Environment.IsEnvironment(EnvironmentNames.Local) || app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseBsui(app.Configuration);
app.UseSecurityService(app.Environment);
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseIdAManAuthentication(app.Configuration);
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage(CommonRouteFor.Host);
app.Run();
