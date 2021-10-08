using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FuelTracker.Models;

namespace FuelTracker.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(Item item, CancellationToken cancellationToken = default);
        Task<bool> DeleteItemAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Item> GetItemAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Item>> GetItemsAsync(bool forceRefresh = false, CancellationToken cancellationToken = default);
    }
}
