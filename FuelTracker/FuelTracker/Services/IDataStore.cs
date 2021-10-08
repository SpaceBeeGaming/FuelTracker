using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FuelTracker.Models;

namespace FuelTracker.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(RefuelingLogItem item, CancellationToken cancellationToken = default);
        Task<bool> DeleteItemAsync(Guid id, CancellationToken cancellationToken = default);
        Task<RefuelingLogItem> GetItemAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<RefuelingLogItem>> GetItemsAsync(bool forceRefresh = false, CancellationToken cancellationToken = default);
    }
}
