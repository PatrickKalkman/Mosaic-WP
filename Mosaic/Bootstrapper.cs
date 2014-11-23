using System;
using System.Collections.Generic;
using System.Windows.Controls;

using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using Caliburn.Micro.Logging;

using Microsoft.Phone.Controls;

using Mosaic.Common;
using Mosaic.Model;
using Mosaic.ViewModels;

using NameIt.Common;

using Telerik.Windows.Controls;

namespace Mosaic
{
    public class Bootstrapper : PhoneBootstrapper
    {
        private PhoneContainer container;

        private RadDiagnostics diagnostics;

        public Bootstrapper()
        {
            LogManager.GetLog = type => new DebugLogger(type);
        }

        void diagnostics_ExceptionOccurred(object sender, ExceptionOccurredEventArgs e)
        { 
            GoogleAnalytics.EasyTracker.GetTracker().SendException(e.Exception.Message, true);
        }

        protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
        {
            return new TransitionFrame();
        }

        protected override void Configure()
        {
            container = new PhoneContainer();

            container.RegisterPhoneServices(RootFrame);

            container.PerRequest<BackgroundImageBrush>();
            container.PerRequest<FlipTileCreator>();
            container.PerRequest<PrivacyPageViewModel>();
            container.PerRequest<MainPageViewModel>();
            container.PerRequest<AutomaticMosaicPageViewModel>();
            container.PerRequest<AdvancedMosaicViewModel>();
            container.PerRequest<ImageStorage>();
            container.PerRequest<FilenameGenerator>();
            container.PerRequest<MultiPhotoChooseViewModel>();
            container.PerRequest<AdvancedMosaicSelectViewModel>();
            container.PerRequest<AboutViewModel>();
            container.PerRequest<HelpPageViewModel>();
            container.RegisterSingleton(typeof(MosaicInformation), "", typeof(MosaicInformation));
            AddCustomConventions();

            diagnostics = new RadDiagnostics();
            diagnostics.EmailTo = "pkalkie@gmail.com";
            diagnostics.ExceptionOccurred += diagnostics_ExceptionOccurred;
            diagnostics.Init();

            ApplicationUsageHelper.Init("1.0");
        }

        static void AddCustomConventions()
        {
            ConventionManager.AddElementConvention<BindableAppBarButton>(
                Control.IsEnabledProperty, "DataContext", "Click");
            ConventionManager.AddElementConvention<BindableAppBarMenuItem>(
                Control.IsEnabledProperty, "DataContext", "Click");
        }

        protected override void OnActivate(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Bootstrapper", "Activate", null, 0);
            base.OnActivate(sender, e);
        }

        protected override void OnLaunch(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Bootstrapper", "Launch", null, 0);
            base.OnLaunch(sender, e);
        }

        protected override void OnDeactivate(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Bootstrapper", "Deactivate", null, 0);
            base.OnDeactivate(sender, e);
        }

        protected override void OnClose(object sender, Microsoft.Phone.Shell.ClosingEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Bootstrapper", "Close", null, 0);
            base.OnClose(sender, e);
        }

        protected override void OnUnhandledException(object sender, System.Windows.ApplicationUnhandledExceptionEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendException(e.ExceptionObject.Message, false);
            base.OnUnhandledException(sender, e);
        }
        
        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}