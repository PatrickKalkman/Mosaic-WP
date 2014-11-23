using System.IO;
using System.Windows.Media.Imaging;

namespace Mosaic.Model
{
    public class ImageAlbum
    {
        public string Name { get; set; }
        public BitmapImage Image { get; set; }
        public WriteableBitmap WritableBitmap { get; set; }
    }
}