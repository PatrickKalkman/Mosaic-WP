using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Media;

namespace Mosaic.Common
{
    public static class EmulatorHelper
    {
        const string flagName = "__emulatorTestImagesAdded";

        public static void AddDebugImages()
        {
            bool alreadyAdded = CheckAlreadyAdded();
            if (!alreadyAdded)
            {
                AddImages();
                SetAddedFlag();
            }
        }

        private static bool CheckAlreadyAdded()
        {
            IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

            try
            {
                bool alreadyAdded = (bool)userSettings[flagName];
                return alreadyAdded;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private static void SetAddedFlag()
        {
            IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
            userSettings.Add(flagName, true);
            userSettings.Save();
        }

        private static void AddImages()
        {
            string[] fileNames = { "WP_20130209_001", "WP_20130224_001", "WP_20130224_001", "WP_20130226_005", "WP_20130309_008" };
            //foreach (var fileName in fileNames)
            //{
            //    MediaLibrary myMediaLibrary = new MediaLibrary();
            //    Uri myUri = new Uri(String.Format(@"TestImages/{0}.jpg", fileName), UriKind.Relative);

            //    System.IO.Stream photoStream = App.GetResourceStream(myUri).Stream;
            //    byte[] buffer = new byte[photoStream.Length];
            //    photoStream.Read(buffer, 0, Convert.ToInt32(photoStream.Length));
            //    myMediaLibrary.SavePicture(String.Format("{0}.jpg", fileName), buffer);
            //    photoStream.Close();
            //}
        }
    }
}
