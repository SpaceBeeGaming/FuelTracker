using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FuelTracker.Models;

using Newtonsoft.Json;
using System.Threading;
using Xamarin.Essentials;

namespace FuelTracker.Services
{
    public class JsonDataStore<T> : IDataStore<T> where T : BaseLogItem
    {
        private List<T>? items;
        private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, typeof(T).Name + "_store.json");

        public JsonDataStore()
        {
            // Migrate old store file.
            string oldPath = Path.Combine(FileSystem.AppDataDirectory, "store.json");
            if (File.Exists(oldPath))
            {
                File.Copy(oldPath, Path.Combine(FileSystem.AppDataDirectory, nameof(RefuelingLogItem) + "_store.json"));
                File.Delete(oldPath);
            }
        }

        public async Task<bool> AddItemAsync(T item, CancellationToken cancellationToken = default)
        {
            (items ??= await LoadItems(cancellationToken)).Add(item);

            return await SaveItems(cancellationToken);
        }


        public async Task<bool> DeleteItemAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _ = items ??= await LoadItems(cancellationToken);

            return items.Remove(items.FirstOrDefault(x => x.Id == id)) is true
                && await SaveItems(cancellationToken);
        }

        public async Task<T> GetItemAsync(Guid id, CancellationToken cancellationToken = default)
            => (items ??= await LoadItems(cancellationToken)).FirstOrDefault(x => x.Id == id);

        public async Task<List<T>> GetItemsAsync(bool forceRefresh = false, CancellationToken cancellationToken = default)
        {
            if (forceRefresh)
            {
                items = null;
            }

            return items ??= await LoadItems(cancellationToken);
        }

        private async Task<List<T>> LoadItems(CancellationToken cancellationToken = default)
        {
            if (File.Exists(filePath) is false)
            {
                await SaveItems(cancellationToken);
            }

            string json = await File.ReadAllTextAsync(filePath, cancellationToken);
            List<T>? list = JsonConvert.DeserializeObject<List<T>>(json);
            if (list is not null)
            {
                list = list.OrderByDescending(x => x.Date).ToList();
            }

            return await Task.FromResult(list ?? new List<T>());
        }

        private async Task<bool> SaveItems(CancellationToken cancellationToken = default)
        {
            string json = JsonConvert.SerializeObject(items, Formatting.Indented);

            await File.WriteAllTextAsync(filePath, json, cancellationToken);

            return true;
        }
    }
}
