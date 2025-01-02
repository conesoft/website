using Conesoft.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddHostConfigurationFiles()
    .AddUsersWithStorage()
    .AddHostEnvironmentInfo()
    .AddLoggingService()
    ;

builder.Services
    .AddCompiledHashCacheBuster()
    .AddHttpClient();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<UserTokenStorage>();

var app = builder.Build();

app
    .UseCompiledHashCacheBuster();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapStaticAssets();

app.MapUsersWithStorage();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();




class UserTokenStorage(HostEnvironment environment)
{
    public Conesoft.Files.Directory Directory => environment.Global.Storage / "Users";
    public Conesoft.Files.Directory For(string User) => Directory / User / "tokens";
}
