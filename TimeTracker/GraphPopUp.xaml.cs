using System;
using System.Collections.Generic;
using Microcharts;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using TimeTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        public GraphPopUp(List<ChartEntry> entries)
        {
            InitializeComponent();
            BindingContext = new GraphPopUpViewModel(entries);
        }
        
    }
}