using System.Threading.Tasks;

using Xamarin.Essentials;

namespace FuelTracker.Services
{
    public class ShareService
    {
        public static async Task ShareFile(string filePath)
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Fuel Tracker data.",
                File = new ShareFile(filePath)
            });
        }
    }
}