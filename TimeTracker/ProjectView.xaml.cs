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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Task = TimeTracker.ViewModels.Task;

namespace TimeTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public class ProjectViewModel : BaseViewModel
    {
        private String _title;
        private readonly ProjectView parent;
        private ObservableCollection<Task> _tasks;

        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set => SetValue(ref _tasks, value);
        }

        public String Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        public ProjectViewModel(ProjectView p)
        {
            this.parent = p;
            Title = parent.project.Title;
            _tasks = parent.project.Tasks;
        }

        public ICommand NewTask
        {
            get
            {
                return new Command(() =>
                {
                    Task t = new Task("Nouvelle tâche", parent.project);
                    Tasks.Add(t);
                    t.IsEdited = true;
                });
            }
        }
        
        public List<ChartEntry> BuildProjectEntries(SKColor textColor, int max_label_length)
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            foreach (var task in Tasks)
            {
                ChartEntry chartEntry = new ChartEntry(task.Duration.Minutes+1); // On commence tjr à 1 
                chartEntry.Color = SKColor.Parse(task.Color.ToHex());
                if (task.Title.Length > max_label_length) chartEntry.Label = task.Title.Substring(0, max_label_length-3) + "...";
                else chartEntry.Label = task.Title;
                chartEntry.TextColor = textColor;
                chartEntry.ValueLabelColor = textColor;
                entries.Add(chartEntry);
            }
            
            return entries;
        }
        
        public ICommand ShowGraph
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new GraphPopUp(BuildProjectEntries(SKColors.White, 20)));
                });
            }
        }
    }
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