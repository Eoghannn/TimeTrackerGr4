using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Microcharts;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using TimeTracker.API;
using TimeTracker.API.Accounts;
using TimeTracker.API.Projects;
using TimeTracker.Model;
using TimeTracker.ViewModels.ListViewItems;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
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

        public ICommand GoToProfil => new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new Profile(this)));
        public ICommand ButtonClicked => new Command(async ()=> await Application.Current.MainPage.Navigation.PushPopupAsync(new GraphPopUp(BuildProjectEntries(SKColors.White, 20))));

        public ICommand NewProject
        {
            get
            {
                return new Command(async () =>
                {
                    Response<ProjectItem> response = await ApiSingleton.Instance.Api.createprojetAsync( ApiSingleton.Instance.access_token, "Nouveau Projet", "");
                    // TODO : vérifier erreur
                    Project p = new Project(this, response);
                    Projects.Add(p);
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
            Response<List<ProjectItem>> listProjets = await ApiSingleton.Instance.Api.getprojetsAsync(ApiSingleton.Instance.access_token);
            // TODO : vérifier erreur
            int i = 0;
            Projects.Clear();
            if (listProjets.Data != null && listProjets.Data.Count > 0)
            {
                foreach (ProjectItem projets in listProjets.Data)
                {
                    Project p = new Project(projets.Name, this, projets.Id);
                    Projects.Add(p);
                    Response<List<TaskItem>> listTask = await ApiSingleton.Instance.Api.gettasksAsync(ApiSingleton.Instance.access_token, projets.Id);
                    // TODO : vérifier erreur
                    foreach (TaskItem tasks in listTask.Data)
                    {
                        Task t = new Task(tasks.Name, p, tasks.Id);
                        Projects[i].AddTask(t);
                        foreach (var time in tasks.Times)
                        {
                            Debug.WriteLine(t.taskId + "; " +time.StartTime + " / "+time.EndTime);
                            t.addTimes(new Times(t, time.Id, time.StartTime, time.EndTime));
                        }
                        t.UpdateDurationText();
                    }

                    i++;
                }
            }

            Response<UserProfileResponse> userProfileResponse = await ApiSingleton.Instance.Api.getmeAsync(ApiSingleton.Instance.access_token);
            if (userProfileResponse.IsSucess)
            {
                nomPrenomString = userProfileResponse.Data.FirstName + " " + userProfileResponse.Data.LastName;
                EmailString = userProfileResponse.Data.Email;
            }
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

        private String _nomPrenomString;

        public String nomPrenomString
        {
            get => _nomPrenomString;
            set => SetValue(ref _nomPrenomString, value);
        }
        
        private String _emailString;

        public String EmailString
        {
            get => _emailString;
            set => SetValue(ref _emailString, value);
        }
    }
}