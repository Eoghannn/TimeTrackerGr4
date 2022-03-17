using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microcharts;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using Storm.Mvvm.Services;
using TimeTracker.ViewModels;
using TimeTracker.ViewModels.ListViewItems;
using Xamarin.Forms;
using Task = TimeTracker.ViewModels.ListViewItems.Task;

namespace TimeTracker
{
    public class ColorList : List<Color>{} // pour récupérer la liste de couleur du xaml, j'ai pas trouvé comment faire sans :(
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(newProjectButton, ProjectColor);
        }
    }
}
