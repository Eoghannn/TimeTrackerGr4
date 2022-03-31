using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Microcharts;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using TimeTracker.API;
using TimeTracker.API.Projects;
using TimeTracker.Model;
using TimeTracker.ViewModels.ListViewItems;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private string _token;
        private Api _api;
        public void RemoveAllTasks(Collection<Task> tasks)
        {
            foreach (var task in tasks)
            {
                if (task == LastTask)
                {
                    _lastTask = null;
                }
            }
        }

        private EditableObject _inEdition; // il ne peut y avoir qu'un project en édition à la fois
        
        public EditableObject InEdition
        {
            get => _inEdition;
            set
            {
                newProjectButton.IsEnabled = value == null;
                SetValue(ref _inEdition, value);
            }
        }
        

        private ObservableCollection<Project> _projects;
        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set => SetValue(ref _projects, value);
        } // static car il ne peut y avoir qu'une seule liste de projects / utilisateur + pour pouvoir y accéder de n'importe ou 
        

        private String _lastTaskNotEmpty;
        public readonly List<Color> ColorList;
        
        // REF DES BUTTONS SUR LA VUE POUR POUVOIR LES NOTIFIER 
        public readonly Button newProjectButton;
        private Task _lastTask;

        public String LastTaskNotEmpty
        {
            get => _lastTaskNotEmpty;
            set => SetValue(ref _lastTaskNotEmpty, value);
        }

        public MainPageViewModel(Button newProjectButton, ColorList colorList )
        {
            _token = Preferences.Get("access_token", "error_token");
            LastTaskNotEmpty = "False";
            Projects = new ObservableCollection<Project>();
            ColorList = new List<Color>();
            this.newProjectButton = newProjectButton;
            for (int i = 0; i < colorList.Capacity; i++)
            {
                ColorList.Add(colorList[i]);
            }
            PopulateData(); 
        }

        public ICommand GoToProfil => new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new Profile()));
        public ICommand ButtonClicked => new Command(async ()=> await Application.Current.MainPage.Navigation.PushPopupAsync(new GraphPopUp(BuildProjectEntries(SKColors.White, 20))));

        public ICommand NewProject
        {
            get
            {
                return new Command(() =>
                {
                    Project p = new Project("Nouveau Projet", this);
                    Projects.Add(p);
                    // /!\ ne pas éditer avant d'ajouter à la liste sinon pb
                    p.IsEdited = true;
                });
            }
        }

        List<ChartEntry> BuildProjectEntries(SKColor textColor, int max_label_length)
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
        
        public async void PopulateData()
        {
            // TODO y'a plus qu'a peuplé la liste de projet avec les bons projets + leurs tâches avec add task, une task possède .TimesList, une list<Times> qui correspond au nombre de fois où cette tâche a été lancé / stoppé, à set correctement aussi vient le constructeur de Times prévu pour
            Response<List<ProjectItem>> listProjets = await _api.getprojetsAsync(_token);
            int i = 0;
            if (listProjets.Data.Count == 0)
            {
                Projects.Add(new Project("Il n'existe pas de projets pour le moment", this));
            }
            else
            {
                foreach (ProjectItem projets in listProjets.Data)
                {
                    Projects.Add(new Project(projets.Name, this));
                    Response<List<TaskItem>> listTask = await _api.gettasksAsync(_token, projets.Id);
                    foreach (TaskItem tasks in listTask.Data)
                    {
                        Projects[i].AddTask(tasks.Name);
                    }
                }
            }


            /*Projects.Add(new Project("Projet Xamarin", this));
            Projects.Add(new Project("Devoir Outils Exploration Données", this));
            Projects.Add(new Project("Projet Flutter", this));
            Project p = new Project("dazda", this);
            Projects.Add(p);
            Debug.WriteLine(Projects.Count);
            Projects[0].AddTask("Implémentation des vues");
            Projects.Remove(p);
            Debug.WriteLine(Projects.Count);*/
        }

        
        public Task LastTask
        {
            get => _lastTask;
            set
            {
                LastTaskNotEmpty = value == null ? "False" : "True";
                SetValue(ref _lastTask, value);
            }
        }
    }
}