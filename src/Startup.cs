using Microsoft.AspNetCore.Builder;

namespace Conesoft.Website
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
