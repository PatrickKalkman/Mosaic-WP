using System.Windows;

using Caliburn.Micro;

using Mosaic.Common;

using NameIt.Common;

namespace Mosaic.ViewModels
{
    public class SettingsPageViewModel : MosaicViewModel
    {
        private readonly MosaicSettingsManager settingsManager;

        public SettingsPageViewModel(BackgroundImageBrush backgroundImageBrush, INavigationService navigationService, ILog logger, MosaicSettingsManager settingsManager) : base(backgroundImageBrush, navigationService, logger)
        {
            this.settingsManager = settingsManager;
        }

        //public bool UseLearningMode
        //{
        //    get { return settingsManager.UseLearningMode; }
        //    set 
        //    {
        //        settingsManager.UseLearningMode = value;
        //        NotifyOfPropertyChange(() => UseLearningMode);
        //    }
        //}
    }
}
