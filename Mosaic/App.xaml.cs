using System.Windows;

using Microsoft.Devices;
using Microsoft.Phone.Controls;

using Mosaic.Common;

using Telerik.Windows.Controls;

namespace Mosaic
{
    public partial class App : Application
    {
        /// <summary>
        /// Component used to handle unhandle exceptions, to collect runtime info and to send email to developer.
        /// </summary>
		public RadDiagnostics diagnostics;
        
		/// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            InitializeComponent();

#if DEBUG
            if (Microsoft.Devices.Environment.DeviceType == DeviceType.Emulator)
            {
                EmulatorHelper.AddDebugImages();
            }
#endif

        }

    }
}
