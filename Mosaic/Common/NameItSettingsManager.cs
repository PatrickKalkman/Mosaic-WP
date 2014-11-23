using NameIt.Common;

namespace Mosaic.Common
{
    public class MosaicSettingsManager
    {
        private readonly SettingsHelper settingsHelper;

        public MosaicSettingsManager(SettingsHelper settingsHelper)
        {
            this.settingsHelper = settingsHelper;
        }

        //private const string UseVoiceRecognitionKey = "UseVoiceRecognition";

        //public bool UseVoiceRecognition
        //{
        //    get
        //    {
        //        return this.settingsHelper.GetSetting(UseVoiceRecognitionKey, true);
        //    }

        //    set
        //    {
        //        this.settingsHelper.UpdateSetting(UseVoiceRecognitionKey, value);
        //    }
        //}
    }
}