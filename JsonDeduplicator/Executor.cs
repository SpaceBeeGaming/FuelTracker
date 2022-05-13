using JsonDeduplicator.Models;
using JsonDeduplicator.Services;

namespace JsonDeduplicator;

internal class Executor
{
    private readonly ILogger<Executor> _logger;
    private readonly IConfiguration _configuration;
    private readonly JsonService _jsonHandler;

    public Executor(ILogger<Executor> logger, IConfiguration configuration, JsonService jsonHandler)
    {
        _logger = logger;
        _configuration = configuration;
        _jsonHandler = jsonHandler;
    }

    public bool Execute()
    {
        // Working Directory.
        DirectoryInfo directoryInfo = new(_configuration.GetConnectionString("StoragePath"));
        if (directoryInfo.Exists is false)
        {
            _logger.LogCritical("The directory {Directory} doesn't exist.", directoryInfo.FullName);
            return false;
        }

        _logger.LogDebug("Working Directory: {WorkingDirectory}", directoryInfo.FullName);

        // Get .json files.
        FileInfo targetFile = new(Path.Combine(directoryInfo.FullName, "consolidated.json"));
        List<FileInfo> files = directoryInfo.GetFiles("*.json")
            .Where(x => !x.Name.StartsWith("appsettings", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (files.Count is 0)
        {
            _logger.LogWarning("No .json files found in {FilePath}", directoryInfo.FullName);
            return false;
        }

        List<Item>? items = _jsonHandler.LoadItems(files);

        items = _jsonHandler.DeduplicateItems(items);

        _jsonHandler.SaveItems(items, targetFile);

        // Removing the target file from the file collection so it doesn't get deleted along with the source files.
        FileInfo? target = files.SingleOrDefault(x => x.Name is "consolidated.json");
        if (target is not null)
        {
            files.Remove(target);
        }

        DeleteFiles(files);
        return true;
    }

    private void DeleteFiles(List<FileInfo> files)
    {
        _logger.LogInformation("Deleting source files.");
        files.ForEach(x =>
        {
            _logger.LogDebug("Deleting file {FileName}", x.Name);
            x.Delete();
        });
    }
}
