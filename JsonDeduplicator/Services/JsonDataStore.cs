using System.IO;
using System.Text.Json;
using System.Collections.Concurrent;

using JsonDeduplicator.Models;

namespace JsonDeduplicator.Services;

public class JsonDataStore
{
    private readonly ILogger<JsonDataStore> _logger;

    public JsonDataStore(ILogger<JsonDataStore> logger)
    {
        _logger = logger;
    }

    public async Task<List<Item>> LoadItemsAsync(List<FileInfo> files, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing <{NumberOfFiles}> files.", files.Count);
        if (files.Count is 0)
        {
            return await Task.FromException<List<Item>>(new ArgumentException("Input file list may not be empty.", nameof(files)));
        }

        if (files.Count is 1)
        {
            _logger.LogInformation("Only one file present. Consolidation not necessary.");
            Worker.CancellationTokenSource.Cancel();
            return await Task.FromCanceled<List<Item>>(cancellationToken);
        }

        ConcurrentBag<Item> items = new();

        try
        {
            await Parallel.ForEachAsync(files, cancellationToken, async (file, cancellationToken) =>
            {
                _logger.LogInformation("Processing file <{FileName}>.", file.Name);
                List<Item>? deserializedList;
                using (FileStream stream = file.OpenRead())
                {
                    deserializedList = await JsonSerializer.DeserializeAsync<List<Item>>(stream, cancellationToken: cancellationToken);
                }

                if (deserializedList is not null)
                {
                    deserializedList.ForEach(item => items.Add(item));
                }

                _logger.LogDebug("Processed file <{FileName}>", file.Name);
            });
        }
        catch (TaskCanceledException ex)
        {
            if (ex.InnerException is null or TaskCanceledException)
            {
                return await Task.FromCanceled<List<Item>>(cancellationToken);
            }
            else
            {
                throw;
            }
        }

        return items.GroupBy(item => item.Id).Select(items => items.First()).ToList();
    }

    public async Task SaveItemsAsync(List<Item> items, FileInfo file, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Saving <{ItemCount}> items to <{FileName}>", items.Count, file.Name);
            using (FileStream stream = file.OpenWrite())
            {
                await JsonSerializer.SerializeAsync(stream, items, new JsonSerializerOptions()
                {
                    WriteIndented = true
                },
                cancellationToken);
            }

            _logger.LogDebug("Finished saving items to <{FileName}>", file.Name);
        }
        catch (TaskCanceledException ex)
        {
            if (ex.InnerException is null or TaskCanceledException)
            {
                await Task.FromCanceled(cancellationToken);
                return;
            }
            else
            {
                throw;
            }
        }
    }
}
