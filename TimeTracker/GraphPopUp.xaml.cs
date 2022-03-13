using System;
using System.Collections.Generic;
using Microcharts;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly Color _backgroundColor;
        private readonly Color _primaryColor;
        private double _titleFontSize;

        public GraphPopUp(List<ChartEntry> entries)
        {
            InitializeComponent();
            _backgroundColor = (Color) (Application.Current.Resources.ContainsKey("Background")
                ? Application.Current.Resources["Background"]
                : Color.White);
            _titleFontSize = (Double) (Application.Current.Resources.ContainsKey("TitleFontSize")
                ? Application.Current.Resources["TitleFontSize"]
                : Color.White);
            ProjectChart.Chart = new PieChart()
            {
                Entries = entries,
                GraphPosition = GraphPosition.AutoFill,
                LabelMode = LabelMode.RightOnly,
                BackgroundColor = SKColor.Parse(_backgroundColor.ToHex()),
                LabelTextSize = (float) _titleFontSize,
                MinValue = 1,
            };
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}