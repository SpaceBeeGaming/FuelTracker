using System.Threading.Tasks;

using Xamarin.Essentials;

namespace FuelTracker.Services
{
    public class PermissionHandler
    {
        public static async Task<bool> VerifyStoragePermission()
        {
            if (await Permissions.CheckStatusAsync<Permissions.StorageWrite>() is not PermissionStatus.Granted)
            {
                if (await Permissions.RequestAsync<Permissions.StorageWrite>() is not PermissionStatus.Granted)
                {
                    return await Task.FromResult(false);
                }
            }
            return await Task.FromResult(true);
        }
    }
}
