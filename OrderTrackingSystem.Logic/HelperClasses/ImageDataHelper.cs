using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrderTrackingSystem.Logic.HelperClasses
{
    public static class ImageDataHelper
    {
        public static ImageSource GetImageFromBytes(byte[] imageData)
        {
            var image = new BitmapImage();
            using (var ms = new MemoryStream(imageData))
            {
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
