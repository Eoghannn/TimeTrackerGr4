using Storm.Mvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker
{
    public partial class App : MvvmApplication
    {
        public App() : base(() => new Login())
        {
            InitializeComponent();
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
