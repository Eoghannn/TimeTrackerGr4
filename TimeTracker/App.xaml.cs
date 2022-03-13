using Storm.Mvvm;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker
{
    public partial class App : MvvmApplication
    {
        public static Color HighlightColor;
        public static Color DefaultColor;

        public App()
        public App() : base(() => new Login())
        {
            InitializeComponent();
            HighlightColor = (Color) (Application.Current.Resources.ContainsKey("Highlight")
                ? Application.Current.Resources["Highlight"]
                : Color.Aqua);
            DefaultColor = (Color) (Application.Current.Resources.ContainsKey("Primary")
                ? Application.Current.Resources["Primary"]
                : Color.White);
            //MainPage = new NavigationPage(new MainPage());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
