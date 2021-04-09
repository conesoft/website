using Conesoft.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Conesoft.Website
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddUsers("Conesoft Host", Hosting.Host.GlobalStorage / "Users");
            services.AddRazorPages();
            services.AddHttpClient();
            services.AddSingleton<Data.Scheduler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseUsers();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}