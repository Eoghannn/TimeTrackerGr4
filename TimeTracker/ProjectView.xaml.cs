using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microcharts;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using TimeTracker.ViewModels;
using TimeTracker.ViewModels.ListViewItems;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Task = TimeTracker.ViewModels.ListViewItems.Task;

namespace TimeTracker
{
    public partial class ProjectView : ContentPage
    {
        public readonly Project project;

        public static ListView currentTaskListView;
        public ProjectView(Project p)
        {
            this.project = p;
            InitializeComponent();
            BindingContext = new ProjectViewModel(this);
            currentTaskListView = taskListView;
        }
    }
}