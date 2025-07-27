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
    .AddCompiledHashCacheBuster()
    .AddHostingDefaults()
    ;

builder.Services
    .AddSingleton<UserTokenStorage>()
    ;

var app = builder.Build();

app.UseCompiledHashCacheBuster();
app.MapPwaInformationFromAppSettings();
app.MapUsersWithStorage();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapGet("/user/login", () => Results.Redirect("/"));

app.Run();