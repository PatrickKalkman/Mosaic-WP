using Caliburn.Micro;

using Mosaic.Resources;

using NameIt.Common;

namespace Mosaic.ViewModels
{
    public class HelpPageViewModel : MosaicViewModel
    {
        public HelpPageViewModel(BackgroundImageBrush backgroundImageBrush, INavigationService navigationService, ILog logger)
            : base(backgroundImageBrush, navigationService, logger)
        {
        }

        public string PageTitle
        {
            get { return AppResources.HelpPageTitle; }
        }
    }
}