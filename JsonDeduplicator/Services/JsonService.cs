using System.Text.Json;

using JsonDeduplicator.Models;

namespace JsonDeduplicator.Services;
public class JsonService
{
    private readonly ILogger<JsonService> _logger;

    public JsonService(ILogger<JsonService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Reads and dematerializes the contents of the .json files.
    /// </summary>
    /// <param name="files">The list of files to gather.</param>
    /// <returns>Returns all of the items found within the <paramref name="files"/>.</returns>
    /// <exception cref="ArgumentException"></exception>
    public List<Item> LoadItems(List<FileInfo> files)
    {
        _logger.LogInformation("Processing {NumberOfFiles} files.", files.Count);
        if (files.Count is 0)
        {
            throw new ArgumentException("Input file list may not be empty.", nameof(files));
        }

        List<Item> items = new();
        foreach (FileInfo file in files)
        {
            List<Item>? result;
            try
            {
                result = JsonSerializer.Deserialize(File.ReadAllText(file.FullName), ItemJsonContext.Default.ListItem);
            }
            catch (JsonException ex)
            {
                _logger.LogCritical(ex, "Json file {File} had errors.", file.FullName);
                throw;
            }

            if (result is null)
            {
                _logger.LogWarning("Processed file {File} was empty.", file.FullName);
                continue;
            }

            items.AddRange(result);
            _logger.LogDebug("Processed file {File}", file.FullName);
        }

        return items;
    }

    /// <summary>
    /// Saves the <paramref name="items"/> to the path specified by <paramref name="file"/>.
    /// </summary>
    /// <param name="items">The items to save.</param>
    /// <param name="file">The file to save to.</param>
    public void SaveItems(List<Item> items, FileInfo file)
    {
        _logger.LogInformation("Saving {ItemCount} entries to {File}", items.Count, file.FullName);
        string result = JsonSerializer.Serialize(items, ItemJsonContext.Default.ListItem);

        File.WriteAllText(file.FullName, result);
    }

    /// <summary>
    /// Deduplicates the <paramref name="items"/> list.
    /// </summary>
    /// <param name="items">List to deduplicate.</param>
    /// <returns>The deduplicated list.</returns>
    public List<Item> DeduplicateItems(List<Item> items)
    {
        List<Item> newItems = items.DistinctBy(item => item.Id).ToList();

        _logger.LogInformation("Deduplicated {BeforeCount} entries down to {AfterCount} entries.", items.Count, newItems.Count);
        return newItems;
    }
}
