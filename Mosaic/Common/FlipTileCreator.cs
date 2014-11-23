using System;
using System.Linq;

using Microsoft.Phone.Shell;

namespace Mosaic.Common
{
    public class FlipTileCreator
    {
        public void CreateTile()
        {
            var navigationUri = CreateNavigationUri();

            FlipTileData tileData = CreateTileData();

            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(t => t.NavigationUri == navigationUri);

            if (tile == null)
            {
                ShellTile.Create(navigationUri, tileData, true);
            }
            else
            {
                tile.Update(tileData);
            }
        }

        public void UpdateDefaultTile(string content, string wideContent)
        {
            FlipTileData tileData = CreateTileData();
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault();
            if (tile != null)
            {
                tile.Update(tileData);
            }
        }

        public void UpdateTile(string content, string wideContent)
        {
            var navigationUri = CreateNavigationUri();

            FlipTileData tileData = CreateTileData();

            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(t => t.NavigationUri == navigationUri);
            if (tile != null)
            {
                tile.Update(tileData);
            }
        }

        private static Uri CreateNavigationUri()
        {
            var navigationUri = new Uri("/Views/AdvancedMosaicSelectView.xaml", UriKind.Relative);
            return navigationUri;
        }

        private static FlipTileData CreateTileData()
        {
            var tileData = new FlipTileData()
            {
                Title = "Photo mosaic",
                BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                SmallBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative),
                WideBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative)
            };
            return tileData;
        }
    }
}
