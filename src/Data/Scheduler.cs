using Conesoft.Files;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Conesoft.Website.Data
{
    public class Scheduler
    {
        static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        readonly Dictionary<string, Json.Task[]> entries = new();
        readonly Directory storage;
        readonly Task loadingAsync;

        async Task OnceLoaded() => await loadingAsync;

        public Scheduler(Directory storage)
        {
            this.storage = storage;
            loadingAsync = LoadFromStorage();
        }

        async Task LoadFromStorage()
        {
            if (storage.Exists)
            {
                foreach (var file in storage.Filtered("*.json", allDirectories: false))
                {
                    entries.Add(file.NameWithoutExtension, await file.ReadFromJson<Json.Task[]>(jsonOptions));
                }
            }
        }

        async Task SafeToStorage(bool overwriteExisting)
        {
            await OnceLoaded();
            if(storage.Exists)
            {
                foreach(var entry in entries)
                {
                    var file = storage / Filename.From(entry.Key, "json");
                    if(file.Exists == false || overwriteExisting == true)
                    {
                        await file.WriteAsJson(entry.Value, jsonOptions);
                    }
                }
            }
        }

        public async Task Add(string website, Json.Task[] tasks)
        {
            await OnceLoaded();
            entries.Remove(website);
            entries.Add(website, tasks);
            await SafeToStorage(overwriteExisting: true);
        }

        public void Clear()
        {
            entries.Clear();
        }

        public IReadOnlyDictionary<string, Json.Task[]> Entries => entries;
    }
}
