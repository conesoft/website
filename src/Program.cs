using Conesoft.Hosting;
using Conesoft.Users;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Toolkit.Uwp.Notifications;
using Serilog;
using System.Web;

var configuration = new ConfigurationBuilder().AddJsonFile(Conesoft.Hosting.Host.GlobalSettings.Path).Build();

var wirepusher = configuration["wirepusher:base"]!;
var wirepusherSecret = configuration["wirepusher:secret"]!;
var conesoftSecret = configuration["conesoft:secret"]!;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCompiledHashCacheBuster()
    .AddPeriodicGarbageCollection(TimeSpan.FromMinutes(5))
    .AddHttpClient()
    .AddUsersWithStorage();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton(new UserTokenStorage(Conesoft.Hosting.Host.GlobalStorage / "Users"));

var app = builder.Build();

app
    .UseCompiledHashCacheBuster();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/notify", async ([FromQuery] string token, [FromQuery] string title, [FromQuery] string message, [FromQuery] string? imageUrl, [FromQuery] string? url) =>
{
    try
    {
        if (token != conesoftSecret)
        {
            return;
        }

        Log.Information("notifying {title}, {message}, {imageUrl}, {url}", title, message, imageUrl, url);

        title = HttpUtility.HtmlDecode(title);
        message = HttpUtility.HtmlDecode(message);

        var wirepusherNotification = new QueryBuilder
        {
            { "title", title ?? "Conesoft Notification" },
            { "message", message },
            { "type", "Server" },
            { "id", wirepusherSecret }
        };

        var localNotification = new ToastContentBuilder()
            .AddText(title, AdaptiveTextStyle.Title)
            .AddText(message);

        if (url != null)
        {
            wirepusherNotification.Add("action", url);
            localNotification = localNotification.SetProtocolActivation(new Uri(url));
        }

        if (imageUrl != null)
        {
            var path = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(imageUrl)));

            using var download = await new HttpClient().GetStreamAsync(imageUrl);
            using var file = File.OpenWrite(path);
            await download.CopyToAsync(file);

            wirepusherNotification.Add("image_url", imageUrl);

            localNotification = localNotification.AddHeroImage(new Uri(path, UriKind.Absolute));
        }

        await new HttpClient().GetAsync(wirepusher + wirepusherNotification.ToQueryString());

        localNotification.Show();
    }
    catch (Exception ex)
    {
        Log.Error("error in /notify: {exception}", ex);
        throw;
    }
});

app.MapUsers();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();




record UserTokenStorage(Conesoft.Files.Directory Directory)
{
    public Conesoft.Files.Directory For(string User) => Directory / User / "tokens";
}
