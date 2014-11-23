using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Phone.Controls;

using Mosaic.ViewModels;

using Telerik.Windows.Controls;

namespace Mosaic.Views
{
    public partial class AdvancedMosaicView : PhoneApplicationPage
    {
        public AdvancedMosaicView()
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

        public static double angleBetween2Lines(PinchContactPoints line1, PinchContactPoints line2)
        {
            if (line1 != null && line2 != null)
            {
                double angle1 = Math.Atan2(line1.PrimaryContact.Y - line1.SecondaryContact.Y,
                                           line1.PrimaryContact.X - line1.SecondaryContact.X);
                double angle2 = Math.Atan2(line2.PrimaryContact.Y - line2.SecondaryContact.Y,
                                           line2.PrimaryContact.X - line2.SecondaryContact.X);
                return (angle1 - angle2) * 180 / Math.PI;
            }
            else { return 0.0; }
        }

       // private async void CropEditor_OnMouseEnter(object sender, MouseEventArgs e)
       // {
       //     Point pos = e.GetPosition(this.CropEditor);
       //     Debug.WriteLine(pos.X + " " + pos.Y);

       //     //if crop button pressed 
       //     if (pos.X > 1 && pos.X < 68)
       //     {
       //         if (pos.Y > 402 && pos.Y < 475)
       //         {
       //             var context = this.DataContext as AdvancedMosaicViewModel;
       //             if (context != null)
       //             {
       //                 context.ImageToSave = await this.CropEditor.CurrentTool.Apply(context.MosaicResult);
       //                 context.OriginalImageSource = context.ImageToSave;
       //                 context.FilterPanelVisible = true;
       //                 context.CropPanelVisible = false;
       //             }
       //         }
       //     }

       //     //if cancel crop button pressed 
       //     if (pos.X > 409 && pos.X < 460)
       //     {
       //         if (pos.Y > 468 && pos.Y < 538)
       //         {
       //             var context = this.DataContext as AdvancedMosaicViewModel;
       //             if (context != null)
       //             {
       //                 context.FilterPanelVisible = true;
       //                 context.CropPanelVisible = false;
       //             }
       //         }
       //     }
       //}

    }
}