using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microcharts;
using Microcharts.Forms;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using Storm.Mvvm;
using TimeTracker.ViewModels;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class GraphPopUpViewModel : ViewModelBase
    {
        private readonly Color _backgroundColor;
        private readonly Double _titleFontSize;

        private PieChart _pieChart;
        public PieChart PieChart
        {
            get => _pieChart;
            set => SetProperty(ref _pieChart, value);
        }
        
        public GraphPopUpViewModel(List<ChartEntry> entries)
        {
            _backgroundColor = (Color) (Application.Current.Resources.ContainsKey("Background")
                ? Application.Current.Resources["Background"]
                : Color.White);
            _titleFontSize = (Double) (Application.Current.Resources.ContainsKey("TitleFontSize")
                ? Application.Current.Resources["TitleFontSize"]
                : Color.White);
            PieChart = new PieChart()
            {
                Entries = entries,
                GraphPosition = GraphPosition.AutoFill,
                LabelMode = LabelMode.RightOnly,
                BackgroundColor = SKColor.Parse(_backgroundColor.ToHex()),
                LabelTextSize = (float) _titleFontSize,
                MinValue = 1
            };
        }
        public ICommand Button_Clicked => new Command(async () => { await Application.Current.MainPage.Navigation.PopPopupAsync(); });
    }
}