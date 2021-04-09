using System.Collections.Generic;

namespace Conesoft.Website.Data
{
    public class Scheduler
    {
        private readonly Dictionary<string, Task[]> entries = new();

        public void Add(string website, Task[] tasks)
        {
            entries.Remove(website);
            entries.Add(website, tasks);
        }

        public void Clear()
        {
            entries.Clear();
        }

        public IReadOnlyDictionary<string, Task[]> Entries => entries;
    }
}
