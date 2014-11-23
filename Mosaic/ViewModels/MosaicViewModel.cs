using System.Windows.Media;

using Caliburn.Micro;

using Mosaic.Resources;

using NameIt.Common;

namespace Mosaic.ViewModels
{
    public class MosaicViewModel : Screen
    {
        protected readonly BackgroundImageBrush backgroundImageBrush;
        protected readonly INavigationService navigationService;
        protected readonly ILog logger;

        public MosaicViewModel(BackgroundImageBrush backgroundImageBrush, INavigationService navigationService, ILog logger)
        {
            this.backgroundImageBrush = backgroundImageBrush;
            this.navigationService = navigationService;
            this.logger = logger;
        }

        public ImageBrush BackgroundImageBrush
        {
            get { return backgroundImageBrush.GetBackground(); }
        }

        public string ApplicationName
        {
            get { return AppResources.ApplicationTitle; }
        }

        private bool isBusy = false;

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }
    }
}