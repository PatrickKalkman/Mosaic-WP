using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

using Microsoft.Xna.Framework.Media;

namespace Mosaic.Common
{
    public class ImageStorage
    {
        public void SaveFile(BitmapImage bitmapImage, string fileName)
        {
            var bmp = new WriteableBitmap(bitmapImage);
            SaveFile(bmp, fileName);
        }

        public void SaveFile(WriteableBitmap bitmapImage, string fileName)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = storage.CreateFile(fileName))
                {
                    bitmapImage.SaveJpeg(stream, bitmapImage.PixelWidth, bitmapImage.PixelHeight, 0, 95);
                    stream.Close();
                }
            }
        }

        public void SaveFileToSavedPictures(WriteableBitmap bitmapImage, string fileName)
        {
            if (bitmapImage != null)
            {
                using (var stream = new MemoryStream())
                {
                    bitmapImage.SaveJpeg(stream, bitmapImage.PixelWidth, bitmapImage.PixelHeight, 0, 100);
                    stream.Seek(0, SeekOrigin.Begin);

                    using (var mediaLibrary = new MediaLibrary())
                    {
                        mediaLibrary.SavePicture(fileName, stream);
                    }
                }
            }
        }


        public void SaveFile(Stream stream, string fileName)
        {
            using (var mediaLibrary = new MediaLibrary())
            {
                mediaLibrary.SavePicture(fileName, stream);
            }
        }



    }
}