using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using Windows.Foundation;
using Windows.UI;

using Nokia.Graphics.Imaging;

namespace Mosaic.Imaging
{
    public class TileEffect : CustomEffectBase
    {
        private readonly List<WriteableBitmap> tileImageProviders;
        private readonly Size tileSize;
        private readonly Size outputResolution;
        private readonly bool shuffle;
        private readonly bool useCircles;
        private readonly Random sourceTileIndexGenerator = new Random((int)DateTime.Now.Ticks);

        public TileEffect(List<WriteableBitmap> tileImageProviders, Size tileSize, Size outputResolution, bool shuffle = false, bool useCircles = false)
            : base(new BitmapImageSource(new Bitmap(outputResolution, ColorMode.Bgra8888)))
        {
            this.tileImageProviders = tileImageProviders;
            this.tileSize = tileSize;
            this.outputResolution = outputResolution;
            this.shuffle = shuffle;
            this.useCircles = useCircles;
        }

        protected override void OnProcess(PixelRegion sourcePixelRegion, PixelRegion targetPixelRegion)
        {
            int numberOfHorizontalTiles = (int)Math.Ceiling(targetPixelRegion.ImageSize.Width / tileSize.Width);
            int numberOfVerticalTiles = (int)Math.Ceiling(targetPixelRegion.ImageSize.Height / tileSize.Height);
            int totalNumberOfTiles = numberOfHorizontalTiles * numberOfVerticalTiles;

            for (int tileCounter = 0; tileCounter < totalNumberOfTiles; tileCounter++)
            {
                int targetRow = tileCounter / numberOfHorizontalTiles;
                int targetColumn = tileCounter % numberOfHorizontalTiles;

                int sourceImageProviderIndex = 0;
                if (shuffle)
                {
                    sourceImageProviderIndex = sourceTileIndexGenerator.Next(0, tileImageProviders.Count - 1);
                }
                else
                {
                    sourceImageProviderIndex = tileCounter % tileImageProviders.Count;
                }

                CopyImageSourceToTarget(sourceImageProviderIndex, targetRow, targetColumn, targetPixelRegion);
            }
        }

        private void CopyImageSourceToTarget(int sourceImageProviderIndex, int targetRow, int targetColumn, PixelRegion targetPixelRegion)
        {
            var source = tileImageProviders[sourceImageProviderIndex];

            int numberOfHorizontalPixelsOutOfRange = 0;
            if ((targetColumn + 1) * tileSize.Width > targetPixelRegion.ImageSize.Width)
            {
                numberOfHorizontalPixelsOutOfRange = (int)((targetColumn + 1) * tileSize.Width - targetPixelRegion.ImageSize.Width);
            }

            int numberOfVerticalPixelsOutOfRange = 0;
            if ((targetRow + 1) * tileSize.Height > targetPixelRegion.ImageSize.Height)
            {
                numberOfVerticalPixelsOutOfRange = (int)((targetRow + 1) * tileSize.Height - targetPixelRegion.ImageSize.Height);
            }

            for (int xSource = 0; xSource < (tileSize.Width - numberOfHorizontalPixelsOutOfRange); xSource++)
            {
                for (int ySource = 0; ySource < (tileSize.Height - numberOfVerticalPixelsOutOfRange); ySource++)
                {
                    byte redSource = source.GetPixel(xSource, ySource).R;
                    byte greenSource = source.GetPixel(xSource, ySource).G;
                    byte blueSource = source.GetPixel(xSource, ySource).B;
                    byte alphaSource = source.GetPixel(xSource, ySource).A;

                    int destinationPixelIndex = (int)(targetColumn * tileSize.Width + targetRow * tileSize.Height * outputResolution.Width + ySource * outputResolution.Width + xSource);

                    if (useCircles && IsInsideCircle(30, 30, 30, xSource, ySource))
                    {
                        targetPixelRegion.ImagePixels[destinationPixelIndex] = FromColor(new Color { A = alphaSource, B = blueSource, G = greenSource, R = redSource });
                    }
                    else if (!useCircles)
                    {
                        targetPixelRegion.ImagePixels[destinationPixelIndex] = FromColor(new Color { A = alphaSource, B = blueSource, G = greenSource, R = redSource });
                    }
                }
            }
        }

        private bool IsInsideCircle(int xCenter, int yCenter, int radius, int x, int y)
        {
            double distance = Math.Sqrt(Math.Pow(xCenter - x, 2) + Math.Pow(yCenter - y, 2));
            return distance < radius;
        }
    }
}
