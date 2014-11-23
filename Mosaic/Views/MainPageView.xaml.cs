using System;

using Microsoft.Phone.Controls;

using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Mosaic.Views
{
    public partial class MainPageView : PhoneApplicationPage
    {
        // Constructor
        public MainPageView()
        {
            InitializeComponent();
        }

		/// <summary>
        /// Navigates to about page.
        /// </summary>
        private void GoToAbout(object sender, GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
