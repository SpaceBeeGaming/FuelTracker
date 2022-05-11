using System.Text.Json;

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
    /// <typeparam name="T">The type of data contained in the <paramref name="files"/>.</typeparam>
    /// <param name="files">The list of files to gather.</param>
    /// <returns>Returns all of the items found within the <paramref name="files"/>.</returns>
    /// <exception cref="ArgumentException"></exception>
    public List<T> LoadItems<T>(List<FileInfo> files)
    {
        _logger.LogInformation("Processing {NumberOfFiles} files.", files.Count);
        if (files.Count is 0)
        {
            throw new ArgumentException("Input file list may not be empty.", nameof(files));
        }

        List<T> items = new();
        foreach (FileInfo file in files)
        {
            List<T>? result;
            try
            {
                result = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(file.FullName));
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
    /// <typeparam name="T">The type of item to save.</typeparam>
    /// <param name="items">The items to save.</param>
    /// <param name="file">The file to save to.</param>
    public void SaveItems<T>(List<T> items, FileInfo file)
    {
        _logger.LogInformation("Saving {ItemCount} entries to {File}", items.Count, file.FullName);
        string result = JsonSerializer.Serialize(items, new JsonSerializerOptions()
        {
            WriteIndented = true,
        });

        File.WriteAllText(file.FullName, result);
    }

    /// <summary>
    /// Deduplicates the <paramref name="items"/> list based on the <paramref name="groupingPredicate"/>.
    /// </summary>
    /// <typeparam name="T">Type contained in the <paramref name="items"/> list.</typeparam>
    /// <typeparam name="TKey">Type of the grouping key.</typeparam>
    /// <param name="items">List to deduplicate.</param>
    /// <param name="groupingPredicate">Grouping function.</param>
    /// <returns>The deduplicated list.</returns>
    public List<T> DeduplicateItems<T, TKey>(List<T> items, Func<T, TKey> groupingPredicate)
    {
        int oldCount = items.Count;
        items = items.GroupBy(groupingPredicate).Select(items => items.First()).ToList();
        int newCount = items.Count;

        _logger.LogInformation("Deduplicated {BeforeCount} entries down to {AfterCount} entries", oldCount, newCount);
        return items;
    }
}
