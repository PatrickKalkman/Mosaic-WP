using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Caliburn.Micro;

using Microsoft.Phone.Tasks;

using Mosaic.Common;
using Mosaic.Model;

using NameIt.Common;

using Nokia.Graphics.Imaging;

namespace Mosaic.ViewModels
{
    public class AdvancedMosaicSelectViewModel : MosaicViewModel, IHandle<SelectedItemsEvent>
    {
        private readonly MosaicInformation mosaicInformation;
        private readonly IEventAggregator eventAggregator;
        private readonly FlipTileCreator flipTileCreator;
        private readonly PhotoChooserTask photoChooserTask = new PhotoChooserTask();

        public AdvancedMosaicSelectViewModel(BackgroundImageBrush backgroundImageBrush, INavigationService navigationService, ILog logger, MosaicInformation mosaicInformation, IEventAggregator eventAggregator, FlipTileCreator flipTileCreator)
            : base(backgroundImageBrush, navigationService, logger)
        {
            this.mosaicInformation = mosaicInformation;
            this.eventAggregator = eventAggregator;
            this.flipTileCreator = flipTileCreator;
            photoChooserTask.ShowCamera = true;
            photoChooserTask.Completed += photoChooserTask_Completed;
            eventAggregator.Subscribe(this);
            PinImageText = "pin";
            PinImageIcon = new Uri("Assets/pin.png", UriKind.Relative);
        }

        public void Handle(SelectedItemsEvent message)
        {
            mosaicInformation.Clear();
            foreach (ImageAlbum chosenPhoto in message.Items)
            {
                mosaicInformation.Add(chosenPhoto.WritableBitmap);
            }

            if (mosaicInformation.Sources.Count > 0 && mosaicInformation.SourceImage != null)
            {
                IsCalculateEnabled = true;
            }
        }

        public void ImageToMosaic()
        {
            photoChooserTask.Show();
        }

        public void MosaicImages()
        {
            navigationService.Navigate(new Uri("/Views/MultiPhotoChooseView.xaml", UriKind.Relative));
        }

        public void CalculateMosaic()
        {
            navigationService.Navigate(new Uri("/Views/AdvancedMosaicView.xaml", UriKind.Relative));
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            try
            {
                if (e.TaskResult == Microsoft.Phone.Tasks.TaskResult.OK)
                {
                    IsBusy = true;
                    var bitmap = new BitmapImage();
                    bitmap.SetSource(e.ChosenPhoto);
                    mosaicInformation.Height = bitmap.PixelHeight;
                    mosaicInformation.Width = bitmap.PixelWidth;
                    e.ChosenPhoto.Position = 0;
                    mosaicInformation.SourceImage = new StreamImageSource(e.ChosenPhoto);
                    if (mosaicInformation.Sources.Count > 0 && mosaicInformation.SourceImage != null)
                    {
                        IsCalculateEnabled = true;
                    }
                }
            }
            catch (Exception error)
            {
                int i = 10;   
                throw;
            }
        }

        public void Pin()
        {
            flipTileCreator.CreateTile();
        }
        
        public string PageTitle
        {
            get { return Resources.AppResources.AdvancedMosaicSelectViewTitle; }
        }
        
        private bool isCalculateEnabled;

        public bool IsCalculateEnabled
        {
            get { return isCalculateEnabled; }
            set
            {
                isCalculateEnabled = value;
                NotifyOfPropertyChange(() => IsCalculateEnabled);
            }
        }

        private Uri pinImageIcon;

        public Uri PinImageIcon
        {
            get { return pinImageIcon; }
            set
            {
                pinImageIcon = value;
                NotifyOfPropertyChange(() => PinImageIcon);
            }
        }

        private string pinImageText;

        public string PinImageText
        {
            get { return pinImageText; }
            set
            {
                pinImageText = value;
                NotifyOfPropertyChange(() => PinImageText);
            }
        }

    }
}