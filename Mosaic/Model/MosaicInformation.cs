using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Nokia.Graphics.Imaging;

namespace Mosaic.Model
{
    public class MosaicInformation
    {
        private readonly List<WriteableBitmap> imageSourcesList = new List<WriteableBitmap>();
        public void Clear()
        {
            imageSourcesList.Clear();
        }

        public void Add(WriteableBitmap imageSourceToAdd)
        {
            imageSourcesList.Add(imageSourceToAdd);
        }

        public List<WriteableBitmap> Sources
        {
            get { return imageSourcesList; }
        }

        public StreamImageSource SourceImage { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
