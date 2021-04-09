using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Conesoft.Website.Api
{
    [Route("[controller]")]
    [ApiController]
    public class TasksController : Controller
    {
        static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [HttpPost("start-scheduling")]
        public async Task<IActionResult> StartScheduling([FromForm] string website, [FromForm] string redirectto, [FromServices] IHttpClientFactory factory, [FromServices] Data.Scheduler scheduler)
        {
            var client = factory.CreateClient();
            var data = await client.GetFromJsonAsync<Data.Task[]>($"https://{website}/tasks/all.json", jsonOptions);

            scheduler.Add(website, data);

            return Redirect(redirectto);
        }
    }
}
