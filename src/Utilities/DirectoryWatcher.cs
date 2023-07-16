using Conesoft.Files;

namespace Conesoft.Website.Utilities;

public static class DirectoryWatcher
{
    public static async Task WatchLive(this Conesoft.Files.Directory directory, Action<Conesoft.Files.File[]> callback)
    {
        await Task.Run(async () =>
        {
            await foreach (var files in directory.Live(allDirectories: true).Changes())
            {
                callback(files.All);
            }
        });
    }
}
