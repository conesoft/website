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
    .AddHttpClient()
    .AddSingleton<UserTokenStorage>()
    .AddRazorComponents().AddInteractiveServerComponents()
    ;

var app = builder.Build();

app.MapPwaInformationFromAppSettings();
app.MapUsersWithStorage();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapGet("/user/login", () => Results.Redirect("/"));

app.Run();