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
using TimeTracker.ViewModels;
using Xamarin.Forms;
using Task = TimeTracker.ViewModels.Task;

namespace TimeTracker
{

    public class ColorList : List<Color>{}

    public class MainPageViewModel : BaseViewModel
    {
        private readonly MainPage parent;

        private String _lastTaskEmpty;

        private String LastTaskEmpty
        {
            get => _lastTaskEmpty;
            set => SetValue(ref _lastTaskEmpty, value);
        }

        public MainPageViewModel(MainPage p)
        {
            this.parent = p;
            LastTaskEmpty = "False";
        }
    }

    public partial class MainPage : ContentPage
    {
        public static ObservableCollection<Project> Projects{ get; set; }
        public static ObservableCollection<Task> lastTask { get; set; }
        
        public static List<Color> ColorList;
        public static ListView PListView;
        public static Button nPButton;
        public MainPage()
        {
            Projects = new ObservableCollection<Project>();
            ColorList = new List<Color>();
            lastTask = new ObservableCollection<Task>();

            InitializeComponent();
            BindingContext = new MainPageViewModel(this);
            PListView = projectListView;
            nPButton = newProjectButton;
            for (int i = 0; i < ProjectColor.Capacity; i++)
            {
                ColorList.Add(ProjectColor[i]);
            }
            
            // TODO y'a plus qu'a peuplé la liste de projet avec les bons projets + leurs tâches avec add task, une task possède .TimesList, une list<Times> qui correspond au nombre de fois où cette tâche a été lancé / stoppé, à set correctement aussi vient le constructeur de Times prévu pour
            Projects.Add(new Project("Projet Xamarin"));
            Projects.Add(new Project("Devoir Outils Exploration Données"));
            Projects.Add(new Project("Projet Flutter"));
            
            Projects[0].AddTask("Implémentation des vues");
            RefreshLastTask();
        }

        public static void RefreshLastTask()
        {
            lastTask.Clear();
            Task t = Task.LastTask();
            if (t != null)
            {
                lastTask.Add(t);
            }
        }
        
        public List<ChartEntry> BuildProjectEntries(SKColor textColor, int max_label_length)
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            foreach (var project in Projects)
            {
                ChartEntry chartEntry = new ChartEntry(project.TotalDuraction().Minutes+1); // On commence tjr à 1 
                chartEntry.Color = SKColor.Parse(project.Color.ToHex());
                if (project.Title.Length > max_label_length) chartEntry.Label = project.Title.Substring(0, max_label_length-3) + "...";
                else chartEntry.Label = project.Title;
                chartEntry.TextColor = textColor;
                chartEntry.ValueLabelColor = textColor;
                entries.Add(chartEntry);
            }
            
            return entries;
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new GraphPopUp(BuildProjectEntries(SKColors.White, 20)));
        }

        private void New_Project(object sender, EventArgs e)
        {
            Project p = new Project("Nouveau Projet");
            Projects.Add(p);
            // /!\ ne pas éditer avant d'ajouter à la liste sinon pb
            p.IsEdited = true;
        }
        
        
    }
}
