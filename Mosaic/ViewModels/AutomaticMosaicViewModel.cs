using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Windows.Foundation;
using Windows.Storage;

using Caliburn.Micro;

using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

using Mosaic.Common;
using Mosaic.Imaging;
using Mosaic.Resources;
using Mosaic.Views;

using NameIt.Common;

using Nokia.Graphics.Imaging;

using Telerik.Windows.Controls;

namespace Mosaic.ViewModels
{
    public class AutomaticMosaicPageViewModel : MosaicViewModel
    {
        private readonly ImageStorage imageStorage;
        private readonly FilenameGenerator filenameGenerator;
        private readonly PhotoChooserTask photoChooserTask = new PhotoChooserTask();
        private int width;
        private int height;

        public AutomaticMosaicPageViewModel(BackgroundImageBrush backgroundImageBrush, INavigationService navigationService, ILog logger, ImageStorage imageStorage, FilenameGenerator filenameGenerator)
            : base(backgroundImageBrush, navigationService, logger)
        {
            this.imageStorage = imageStorage;
            this.filenameGenerator = filenameGenerator;
            photoChooserTask.ShowCamera = true;
            photoChooserTask.Completed += photoChooserTask_Completed;
            ChooseImageIcon = new Uri("Assets/folder.png", UriKind.Relative);
            ChooseImageText = "choose";
            RefreshImageIcon = new Uri("Assets/refresh.png", UriKind.Relative);
            RefreshImageText = "refresh";
            SaveImageIcon = new Uri("Assets/save.png", UriKind.Relative);
            SaveImageText = "save";
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            if (!IsBusy)
            {
                StartPhotoChooserTask();
            }
        }

        private AutomaticMosaicPage view;

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            this.view = (AutomaticMosaicPage)view;

            var progressIndicator = new ProgressIndicator();
            progressIndicator.IsIndeterminate = true;
            progressIndicator.Text = string.Format("Calculating Mosaic");

            if (sourceImage != null)
            {
                progressIndicator.IsVisible = true;
            }


            SystemTray.SetProgressIndicator(this.view, progressIndicator);
        }

        private void StartPhotoChooserTask()
        {
            IsBusy = true;
            photoChooserTask.Show();
        }

        private WriteableBitmap mosaicResult;

        public async void Process()
        {
            if (sourceImage != null)
            {
                SystemTray.ProgressIndicator.IsVisible = true;
                try
                {
                    //var imageSourcesList = new List<IImageProvider>();
                    //for (int tileIndex = 1; tileIndex < 18; tileIndex++)
                    //{
                    //    string tileName = string.Format(@"Assets\MosaicImages\Tile{0}R.png", tileIndex);
                    //    StorageFile image = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(tileName);
                    //    var imageSource = new StorageFileImageSource(image);
                    //    imageSourcesList.Add(imageSource);
                    //}

                    //mosaicResult = new WriteableBitmap(width, height);

                    //using (var tileEffect = new TileEffect(imageSourcesList, new Size(60, 60), new Size(width, height), true, false))
                    //using (var blendFilter = new BlendFilter(tileEffect) { BlendFunction = BlendFunction.Overlay, Level = 1 })
                    //using (var filterEffect = new FilterEffect(sourceImage) { Filters = new[] { blendFilter } })
                    //using (var renderer = new WriteableBitmapRenderer(filterEffect, mosaicResult))
                    //{
                    //    await renderer.RenderAsync();
                    //    OriginalImageSource = mosaicResult;
                    //    IsBusy = false;
                    //}
                }
                finally
                {
                    SystemTray.ProgressIndicator.IsVisible = false;
                }
            }
        }

        public void RefreshImage()
        {
            IsBusy = true;
            Process();
        }

        public void ChooseImage()
        {
            StartPhotoChooserTask();
        }

        private ImageSource originalImageSource;

        public ImageSource OriginalImageSource
        {
            get { return originalImageSource; }
            set
            {
                originalImageSource = value;
                NotifyOfPropertyChange(() => OriginalImageSource);
            }
        }

        StreamImageSource sourceImage;

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == Microsoft.Phone.Tasks.TaskResult.OK)
            {
                if (e.ChosenPhoto != null)
                {
                    IsBusy = true;
                    var bitmap = new BitmapImage();
                    bitmap.SetSource(e.ChosenPhoto);
                    height = bitmap.PixelHeight;
                    width = bitmap.PixelWidth;
                    e.ChosenPhoto.Position = 0;
                    sourceImage = new StreamImageSource(e.ChosenPhoto);
                    Process();
                }
            }
        }

        public void SaveImage()
        {
            if (sourceImage != null)
            {
                string fileName = filenameGenerator.Generate();
                imageStorage.SaveFileToSavedPictures(mosaicResult, fileName);
                RadMessageBox.ShowAsync(string.Format("The image {0} is saved in your media library.", fileName), "Saved");
            }
        }

        private Uri chooseImageIcon;

        public Uri ChooseImageIcon
        {
            get { return chooseImageIcon; }
            set
            {
                chooseImageIcon = value;
                NotifyOfPropertyChange(() => ChooseImageIcon);
            }
        }

        private string chooseImageText;

        public string ChooseImageText
        {
            get { return chooseImageText; }
            set
            {
                chooseImageText = value;
                NotifyOfPropertyChange(() => ChooseImageText);
            }
        }

        private Uri refreshImageIcon;

        public Uri RefreshImageIcon
        {
            get { return refreshImageIcon; }
            set
            {
                refreshImageIcon = value;
                NotifyOfPropertyChange(() => RefreshImageIcon);
            }
        }

        private string refreshImageText;

        public string RefreshImageText
        {
            get { return refreshImageText; }
            set
            {
                refreshImageText = value;
                NotifyOfPropertyChange(() => RefreshImageText);
            }
        }

        private Uri saveImageIcon;

        public Uri SaveImageIcon
        {
            get { return saveImageIcon; }
            set
            {
                saveImageIcon = value;
                NotifyOfPropertyChange(() => SaveImageIcon);
            }
        }

        private string saveImageText;

        public string SaveImageText
        {
            get { return saveImageText; }
            set
            {
                saveImageText = value;
                NotifyOfPropertyChange(() => SaveImageText);
            }
        }

        public string PageTitle
        {
            get { return AppResources.AutomaticPageTitle; }
        }
    }
}