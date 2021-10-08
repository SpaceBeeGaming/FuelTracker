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
    public class JsonDataStore : IDataStore<RefuelingLogItem>
    {
        private List<RefuelingLogItem>? items;
        private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "store.json");


        public async Task<bool> AddItemAsync(RefuelingLogItem item, CancellationToken cancellationToken = default)
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

        public async Task<RefuelingLogItem> GetItemAsync(Guid id, CancellationToken cancellationToken = default)
            => (items ??= await LoadItems(cancellationToken)).FirstOrDefault(x => x.Id == id);

        public async Task<List<RefuelingLogItem>> GetItemsAsync(bool forceRefresh = false, CancellationToken cancellationToken = default)
        {
            if (forceRefresh)
            {
                items = null;
            }

            return items ??= await LoadItems(cancellationToken);
        }

        private async Task<List<RefuelingLogItem>> LoadItems(CancellationToken cancellationToken = default)
        {
            if (File.Exists(filePath) is false)
            {
                await SaveItems(cancellationToken);
            }

            string json = await File.ReadAllTextAsync(filePath, cancellationToken);
            List<RefuelingLogItem>? list = JsonConvert.DeserializeObject<List<RefuelingLogItem>>(json);
            if (list is not null)
            {
                list = list.OrderByDescending(x => x.Date).ToList();
            }

            return await Task.FromResult(list ?? new List<RefuelingLogItem>());
        }

        private async Task<bool> SaveItems(CancellationToken cancellationToken = default)
        {
            string json = JsonConvert.SerializeObject(items, Formatting.Indented);

            await File.WriteAllTextAsync(filePath, json, cancellationToken);

            return true;
        }
    }
}
