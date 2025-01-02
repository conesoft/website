using Conesoft.Files;

namespace Conesoft.Website.Utilities;

public static class DirectoryWatcher
{
    public static async Task WatchLive(this Conesoft.Files.Directory directory, Action<IEnumerable<Conesoft.Files.File>> callback)
    {
        await Task.Run(async () =>
        {
            await foreach (var entries in directory.Changes(allDirectories: true))
            {
                callback(entries.All.Files());
            }
        });
    }
}
