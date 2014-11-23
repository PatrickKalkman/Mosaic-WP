using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Telerik.Windows.Controls;

namespace Mosaic.Views
{
    public partial class AdvancedMosaicSelectView : PhoneApplicationPage
    {
        public AdvancedMosaicSelectView()
        {
            InitializeComponent();
            InteractionEffectManager.AllowedTypes.Add(typeof(Button));
        }
    }
}