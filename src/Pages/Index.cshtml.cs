using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Conesoft.Website.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet([FromServices] Data.Scheduler scheduler)
        {
            Entries = scheduler.Entries;
        }
        public void OnPost([FromServices] Data.Scheduler scheduler)
        {
            scheduler.Clear();
            Entries = null;
        }

        public IReadOnlyDictionary<string, Data.Json.Task[]> Entries { get; private set; }
    }
}