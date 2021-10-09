using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FuelTracker.Services
{
    public interface IDataStore<T> where T : class
    {
        Task<bool> AddItemAsync(T item, CancellationToken cancellationToken = default);
        Task<bool> DeleteItemAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T> GetItemAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<T>> GetItemsAsync(bool forceRefresh = false, CancellationToken cancellationToken = default);
    }
}
