using System;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Phone.Controls;

namespace Mosaic.Views
{
    public partial class AutomaticMosaicPage : PhoneApplicationPage
    {
        public AutomaticMosaicPage()
        {
            InitializeComponent();
        }

        private void Result_OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null)
            {
                var transform = (CompositeTransform)Result.RenderTransform;

                // Scale Manipulation
                transform.ScaleX = e.PinchManipulation.CumulativeScale;
                transform.ScaleY = e.PinchManipulation.CumulativeScale;

                // Translate manipulation
                var originalCenter = e.PinchManipulation.Original.Center;
                var newCenter = e.PinchManipulation.Current.Center;
                transform.TranslateX = newCenter.X - originalCenter.X;
                transform.TranslateY = newCenter.Y - originalCenter.Y;

                // end 
                e.Handled = true;
            }
        }
    }
}