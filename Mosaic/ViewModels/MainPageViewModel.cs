using System;

using Caliburn.Micro;

using NameIt.Common;

namespace Mosaic.ViewModels
{
    public class MainPageViewModel : MosaicViewModel
    {
        public MainPageViewModel(INavigationService navigationService, BackgroundImageBrush backgroundImageBrush, ILog logger) : base(backgroundImageBrush, navigationService, logger)
        {
            AboutImageText = "about";
            HelpImageText = "help";
            HelpImageIcon = new Uri("Assets/help.png", UriKind.Relative);
            AboutImageIcon = new Uri("Assets/about.png", UriKind.Relative);
            PrivacyButtonText = "privacy";
        }

        public void MosaicFast()
        {
            navigationService.Navigate(new Uri("/Views/AutomaticMosaicPage.xaml", UriKind.Relative));
        }

        public void MosaicAdvanced()
        {
            navigationService.Navigate(new Uri("/Views/AdvancedMosaicSelectView.xaml", UriKind.Relative));
        }

        public void About()
        {
            navigationService.Navigate(new Uri("/Views/AboutView.xaml", UriKind.Relative));
        }

        public void Help()
        {
            navigationService.Navigate(new Uri("/Views/HelpPageView.xaml", UriKind.Relative));
        }

        public void Privacy()
        {
            navigationService.Navigate(new Uri("/Views/PrivacyPage.xaml", UriKind.Relative));
        }

        public string PageTitle
        {
            get { return Resources.AppResources.MainPageTitle;  }
        }

        private Uri helpImageIcon;

        public Uri HelpImageIcon
        {
            get { return helpImageIcon; }
            set
            {
                helpImageIcon = value;
                NotifyOfPropertyChange(() => HelpImageIcon);
            }
        }

        private string helpImageText;

        public string HelpImageText
        {
            get { return helpImageText; }
            set
            {
                helpImageText = value;
                NotifyOfPropertyChange(() => HelpImageText);
            }
        }

        private Uri aboutImageIcon;

        public Uri AboutImageIcon
        {
            get { return aboutImageIcon; }
            set
            {
                aboutImageIcon = value;
                NotifyOfPropertyChange(() => AboutImageIcon);
            }
        }

        private string aboutImageText;

        public string AboutImageText
        {
            get { return aboutImageText; }
            set
            {
                aboutImageText = value;
                NotifyOfPropertyChange(() => AboutImageText);
            }
        }

        private string privacyButtonText;

        public string PrivacyButtonText
        {
            get { return privacyButtonText; }
            set
            {
                aboutImageText = value;
                NotifyOfPropertyChange(() => PrivacyButtonText);
            }
        }
    }
}