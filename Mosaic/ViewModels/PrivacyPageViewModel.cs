using Caliburn.Micro;

using Mosaic.Resources;

using NameIt.Common;

namespace Mosaic.ViewModels
{
    public class PrivacyPageViewModel : MosaicViewModel
    {
        public PrivacyPageViewModel(BackgroundImageBrush backgroundImageBrush, INavigationService navigationService, ILog logger)
            : base(backgroundImageBrush, navigationService, logger)
        {
        }

        public string PageTitle
        {
            get { return AppResources.PrivacyPageTitle; }
        }
    }
}