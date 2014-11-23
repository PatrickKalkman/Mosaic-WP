using System.Reflection;

using Caliburn.Micro;

using Microsoft.Phone.Tasks;

using Mosaic.Resources;

using NameIt.Common;

namespace Mosaic.ViewModels
{
    public class AboutViewModel : MosaicViewModel
    {
        public AboutViewModel(BackgroundImageBrush backgroundImageBrush, INavigationService navigationService, ILog logger)
            : base(backgroundImageBrush, navigationService, logger)
        {
        }

        public string PageTitle
        {
            get { return AppResources.AboutPageTitle; }
        }


        public string Version
        {
            get
            {
                var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
                return nameHelper.Version.ToString();
            }
        }

        public string AppDescription
        {
            get
            {
                return "Create beautiful mosaics from your own personal images. Enhance them by adding several filters.";
            }
        }

        public void RateThisApp()
        {
            var reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }

        public void SendAnEmail()
        {
            var emailTask = new EmailComposeTask();
            emailTask.To = "pkalkie@gmail.com";
            emailTask.Show();
        }
    }
}
