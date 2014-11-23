using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Windows.Foundation;
using Windows.Storage.Streams;

using Caliburn.Micro;

using Microsoft.Phone.Shell;

using Mosaic.Common;
using Mosaic.Imaging;
using Mosaic.Model;
using Mosaic.Resources;
using Mosaic.Views;

using NameIt.Common;

using Nokia.Graphics.Imaging;

using Telerik.Windows.Controls;

using GradientStop = Nokia.Graphics.Imaging.GradientStop;

namespace Mosaic.ViewModels
{
    public class AdvancedMosaicViewModel : MosaicViewModel
    {
        private readonly ImageStorage imageStorage;
        private readonly FilenameGenerator filenameGenerator;
        private readonly MosaicInformation mosaicInformation;

        public AdvancedMosaicViewModel(BackgroundImageBrush backgroundImageBrush, INavigationService navigationService, ILog logger, ImageStorage imageStorage, FilenameGenerator filenameGenerator, MosaicInformation mosaicInformation)
            : base(backgroundImageBrush, navigationService, logger)
        {
            this.imageStorage = imageStorage;
            this.filenameGenerator = filenameGenerator;
            this.mosaicInformation = mosaicInformation;
            CropImageIcon = new Uri("Assets/crop2.png", UriKind.Relative);
            CropImageText = "crop";
            RefreshImageIcon = new Uri("Assets/refresh.png", UriKind.Relative);
            RefreshImageText = "refresh";
            SaveImageIcon = new Uri("Assets/save.png", UriKind.Relative);
            SaveImageText = "save";

            toggleContent = "Squares";
            typeOfMosaic = true;
            FilterPanelVisible = true;
            CropPanelVisible = false;
            IsAppBarVisible = true;
        }

        private WriteableBitmap mosaicResult;

        public WriteableBitmap MosaicResult
        {
            get { return mosaicResult; }
        }

        private AdvancedMosaicView view;

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            this.view = (AdvancedMosaicView)view;

            var progressIndicator = new ProgressIndicator();
            progressIndicator.IsIndeterminate = true;
            progressIndicator.IsVisible = true;
            progressIndicator.Text = string.Format("Calculating Mosaic");

            SystemTray.SetProgressIndicator(this.view, progressIndicator);
        }

        public string PageTitle
        {
            get { return AppResources.AdjustPageTitle; }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            IsBusy = true;
            CalculateMosaic();
        }

        private bool inProgress;

        public async void CalculateMosaic()
        {
            if (!inProgress)
            {
                inProgress = true;
                try
                {
                    SystemTray.ProgressIndicator.IsVisible = true;

                    //mosaicResult = new WriteableBitmap(mosaicInformation.Width, mosaicInformation.Height);

                    var filters = CreateFilterList();

                    using (var tileEffect = new TileEffect(mosaicInformation.Sources, new Size(60, 60), new Size(mosaicInformation.Width, mosaicInformation.Height), true, false))
                    using (var blendFilter = new BlendFilter(tileEffect) { BlendFunction = BlendFunction.Overlay, Level = 1 })
                    using (var filterEffect = new FilterEffect(mosaicInformation.SourceImage) { Filters = new[] { blendFilter } })
                    using (var renderer = new JpegRenderer(filterEffect))
                    {
                        IBuffer result = await renderer.RenderAsync();
                        imageStorage.SaveFile(result.AsStream(), "temp.jpg");
                        IsBusy = false;
                    }

                    //using (var tileEffect = new TileEffect(mosaicInformation.Sources, new Size(60, 60), new Size(mosaicInformation.Width, mosaicInformation.Height), true, !TypeOfMosaic))
                    //using (var blendFilter = new BlendFilter(tileEffect) { BlendFunction = BlendFunction.Overlay, Level = 1 })
                    //{
                    //    using (var lensBlurEffect = new LensBlurEffect(mosaicInformation.SourceImage, new LensBlurPredefinedKernel(LensBlurPredefinedKernelShape.Circle, 30) { PointLightStrength = 9 }))
                    //    {
                    //        filters.Add(blendFilter);
                    //        using (var filterEffect = new FilterEffect(lensBlurEffect) { Filters = filters })
                    //        {
                    //            using (var renderer = new WriteableBitmapRenderer(filterEffect, mosaicResult))
                    //            {
                    //                lensBlurEffect.KernelMap = CreateKernelMap();
                    //                await renderer.RenderAsync();
                    //                OriginalImageSource = mosaicResult;
                    //                IsBusy = false;
                    //            }
                    //        }
                    //    }
                    //}

                    SystemTray.ProgressIndicator.IsVisible = false;
                }
                finally
                {
                    SystemTray.ProgressIndicator.IsVisible = false;
                    inProgress = false;
                }
            }
        }

        public void CropImage()
        {
            try
            {
                FilterPanelVisible = false;
                CropPanelVisible = true;
                IsAppBarVisible = false;
                this.view.CropEditor.CurrentTool = this.view.CropEditor.Tools[0];
            }
            catch (Exception error)
            {
                if (logger != null)
                {
                    logger.Error(error);
                }
            }
        }

        private List<IFilter> CreateFilterList()
        {
            var filters = new List<IFilter>();
            if (ApplyLomoFilter)
            {
                IFilter lomo = new LomoFilter(0.7, 0.4, LomoVignetting.Low, LomoStyle.Red);
                filters.Add(lomo);
            }

            if (ApplyGrayScaleFilter)
            {
                IFilter grayScale = new GrayscaleFilter();
                filters.Add(grayScale);
            }

            if (ApplySepiaFilter)
            {
                IFilter sepiaFilter = new SepiaFilter();
                filters.Add(sepiaFilter);
            }

            if (ApplyGradientFilter)
            {
                var colorStops = new[]
                {
                    new GradientStop { Color = Windows.UI.Color.FromArgb(255, 0, 0, 0), Offset = 0.0 },
                    new GradientStop { Color = Windows.UI.Color.FromArgb(255, 100, 100, 100), Offset = 1.0 }
                };

                var gradient = new LinearGradient(new Point(0, 0), new Point(0, 1), colorStops);

                var source = new GradientImageSource(new Size(mosaicInformation.Width, mosaicInformation.Height), gradient);

                var filter = new BlendFilter(source) { BlendFunction = BlendFunction.Overlay, Level = 0.5 };

                filters.Add(filter);
            }

            return filters;
        }

        private GradientImageSource CreateKernelMap()
        {
            GradientStop[] colorStops;

            if (applyBlurFilter)
            {
                colorStops = new[]
                {
                    new GradientStop { Color = Windows.UI.Color.FromArgb(255, 0, 0, 0), Offset = 0.0 },
                    new GradientStop { Color = Windows.UI.Color.FromArgb(255, 0, 0, 0), Offset = 1.0 }
                };
            }
            else
            {
                colorStops = new[]
                {
                    new GradientStop { Color = Windows.UI.Color.FromArgb(255, 255, 255, 255), Offset = 0.0 },
                    new GradientStop { Color = Windows.UI.Color.FromArgb(255, 255, 255, 255), Offset = 1.0 }
                };                
            }

            var gradient = new LinearGradient(new Point(0, 0), new Point(0, 1), colorStops);

            return new GradientImageSource(new Size(mosaicInformation.Width, mosaicInformation.Height), gradient);            
        }

        public void RefreshImage() 
        {
            IsBusy = true;
            CalculateMosaic();
        }

        public async void MouseEnter(MouseEventArgs e)
        {
            System.Windows.Point pos = e.GetPosition(this.view.CropEditor);
            Debug.WriteLine(pos.X + " " + pos.Y);

            //if crop button pressed 
            if (pos.X > 1 && pos.X < 80)
            {
                if (pos.Y > 420 && pos.Y < 527)
                {
                    ImageToSave = await view.CropEditor.CurrentTool.Apply(MosaicResult);
                    if (ImageToSave.PixelWidth != 0 && ImageToSave.PixelHeight != 0)
                    {
                        OriginalImageSource = ImageToSave;
                    }
                    FilterPanelVisible = true;
                    CropPanelVisible = false;
                    IsAppBarVisible = true;
                }
            }

            //if cancel crop button pressed 
            if (pos.X > 360 && pos.X < 428)
            {
                if (pos.Y > 420 && pos.Y < 510)
                {
                    FilterPanelVisible = true;
                    CropPanelVisible = false;
                    IsAppBarVisible = true;
                }
            }
        }


        private bool typeOfMosaic;

        public bool TypeOfMosaic
        {
            get { return typeOfMosaic; }
            set
            {
                typeOfMosaic = value;
                NotifyOfPropertyChange(() => TypeOfMosaic);
                if (value)
                {
                    ToggleContent = "Squares";
                }
                else
                {
                    ToggleContent = "Circles";
                }
                RefreshImage();
            }
        }

        private string toggleContent;

        public string ToggleContent
        {
            get { return toggleContent; }
            set
            {
                toggleContent = value;
                NotifyOfPropertyChange(() => ToggleContent);
            }
        }

        private bool applyGradientFilter;

        public bool ApplyGradientFilter
        {
            get { return applyGradientFilter; }
            set
            {
                applyGradientFilter = value;
                NotifyOfPropertyChange(() => ApplyGradientFilter);
                RefreshImage();
            }
        }

        private bool applyLomoFilter;

        public bool ApplyLomoFilter
        {
            get { return applyLomoFilter; }
            set
            {
                applyLomoFilter = value;
                NotifyOfPropertyChange(() => ApplyLomoFilter);
                RefreshImage();
            }
        }

        private bool applyGrayScaleFilter;

        public bool ApplyGrayScaleFilter
        {
            get { return applyGrayScaleFilter; }
            set
            {
                applyGrayScaleFilter = value;
                NotifyOfPropertyChange(() => ApplyGrayScaleFilter);
                RefreshImage();
            }
        }

        private bool applyBlurFilter;

        public bool ApplyBlurFilter
        {
            get { return applyBlurFilter; }
            set
            {
                applyBlurFilter = value;
                NotifyOfPropertyChange(() => ApplyBlurFilter);
                RefreshImage();
            }
        }

        private bool applySepiaFilter;

        public bool ApplySepiaFilter
        {
            get { return applySepiaFilter; }
            set
            {
                applySepiaFilter = value;
                NotifyOfPropertyChange(() => ApplySepiaFilter);
                RefreshImage();
            }
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

        public void SaveImage()
        {
            string fileName = filenameGenerator.Generate();

            imageStorage.SaveFileToSavedPictures(ImageToSave, fileName);
            RadMessageBox.ShowAsync(string.Format("The image {0} is saved in your media library.", fileName), "Saved");
        }

        private Uri cropImageIcon;

        public Uri CropImageIcon
        {
            get { return cropImageIcon; }
            set
            {
                cropImageIcon = value;
                NotifyOfPropertyChange(() => CropImageIcon);
            }
        }

        private string cropImageText;

        public string CropImageText
        {
            get { return cropImageText; }
            set
            {
                cropImageText = value;
                NotifyOfPropertyChange(() => CropImageText);
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
        
        private bool isAppBarVisible;

        public bool IsAppBarVisible
        {
            get { return isAppBarVisible; }
            set
            {
                isAppBarVisible = value;
                NotifyOfPropertyChange(() => IsAppBarVisible);
            }
        }

        private bool filterPanelVisible;

        public bool FilterPanelVisible
        {
            get { return filterPanelVisible; }
            set
            {
                filterPanelVisible = value;
                NotifyOfPropertyChange(() => FilterPanelVisible);
            }
        }

        private bool cropPanelVisible;

        public bool CropPanelVisible
        {
            get { return cropPanelVisible; }
            set
            {
                cropPanelVisible = value;
                NotifyOfPropertyChange(() => CropPanelVisible);
            }
        }

        public WriteableBitmap ImageToSave { get; set; }
    }
}
