using Conesoft.Hosting;
using Conesoft.PwaGenerator;
using Conesoft.Website.Components;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddHostConfigurationFiles()
    .AddUsersWithStorage()
    .AddHostEnvironmentInfo()
    .AddLoggingService()
    .AddNotificationService()
    ;

builder.Services
    .AddCompiledHashCacheBuster()
    .AddSingleton<UserTokenStorage>()
    .AddHttpClient()
    .AddAntiforgery()
    .AddCascadingAuthenticationState()
    .AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

app
    .UseCompiledHashCacheBuster()
    .UseDeveloperExceptionPage()
    .UseRouting()
    .UseStaticFiles()
    .UseAuthentication()
    .UseAuthorization()
    .UseAntiforgery();

app.MapStaticAssets();

app.MapUsersWithStorage();
app.MapPwaInformationFromAppSettings();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();