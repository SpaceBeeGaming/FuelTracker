using JsonDeduplicator.Models;
using JsonDeduplicator.Services;

namespace JsonDeduplicator;

public class Worker
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly JsonDataStore _dataStore;

    public static CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

    public Worker(ILogger<Worker> logger, IConfiguration configuration, JsonDataStore dataStore)
    {
        _logger = logger;
        _configuration = configuration;
        _dataStore = dataStore;
    }

    public async Task ExecuteAsync(CancellationToken stopToken)
    {
        // Create new CancellationTokenSource so i have manual control over cancellation.
        CancellationToken cancelToken = CancellationTokenSource.Token;
        stopToken.Register(() => CancellationTokenSource.Cancel());

        // Working Directory.
        DirectoryInfo directoryInfo = new(_configuration.GetConnectionString("StoragePath"));
        _logger.LogDebug("Working Directory: {WorkingDirectory}", directoryInfo.FullName);

        // Get .json files.
        List<FileInfo> files = directoryInfo.GetFiles().Where(x => x.Extension is ".json").ToList();
        FileInfo targetFile = new(Path.Combine(directoryInfo.FullName, "fuelTrackerData_consolidated.json"));

        Task<List<Item>> load;
        Task save;
        Task delete;

        try
        {
            // Run the consolidation.
            load = _dataStore.LoadItemsAsync(files, cancelToken);
            save = await load.ContinueWith(async (x) => await _dataStore.SaveItemsAsync(x.Result, targetFile, cancelToken), TaskContinuationOptions.OnlyOnRanToCompletion);
            delete = await save.ContinueWith(_ => DeleteFiles(files), TaskContinuationOptions.OnlyOnRanToCompletion);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Consolidation Canceled");
            return;
        }

        if (delete.IsCompletedSuccessfully)
        {
            _logger.LogInformation("Consolidation completed successfully.");
        }
        else
        {
            _logger.LogError("Consolidation Failed. Reason Unknown");
        }
    }

    private Task DeleteFiles(List<FileInfo> files)
    {
        _logger.LogInformation("Deleting Source files.");
        files.ForEach(x =>
        {
            _logger.LogDebug("Deleting file <{FileName}>", x.Name);
            x.Delete();
        });

        return Task.CompletedTask;
    }
}
