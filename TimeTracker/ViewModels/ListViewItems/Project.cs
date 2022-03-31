using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using TimeTracker.API;
using TimeTracker.API.Projects;
using Xamarin.Forms;

namespace TimeTracker.ViewModels.ListViewItems
{
        public sealed class Project : ProjectTask
    {
        public override bool IsEdited { 
            get
            {
                return base.IsEdited;
            }
            set
            {
                if (!value)
                {
                    // le titre du projet a potentiellement été modifié : informer le serveur ici
                    ApiSingleton.Instance.Api.modifprojetAsync(ApiSingleton.Instance.access_token, Title, "", projectId).ContinueWith((
                        async task =>
                        {
                            Response<ProjectItem> response = await task;
                            // TODO : vérifier erreur
                        })
                    );
                }

                base.IsEdited = value;
            } 
        }
        private ObservableCollection<Task> _tasks;
        public readonly MainPageViewModel MainPageViewModelRef;

        public long projectId;
        public Task StartedObj{ get; set; }


        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
        }

        private void AddTask(String title)
        {
            Task t = new Task(title, this, Tasks.Count);
            AddTask(t);
        }
        public void AddTask(Task t)
        {
            Tasks.Add(t);
        }
        
        
        public Project(string title, MainPageViewModel mainPageViewModelRef, long projectId) : base(mainPageViewModelRef)
        {
            Title = title;
            MainPageViewModelRef = mainPageViewModelRef;
            this.projectId = projectId;
            _tasks = new ObservableCollection<Task>();
            if (mainPageViewModelRef.ColorList.Count == 0) Color = Color.White;
            else Color = mainPageViewModelRef.ColorList[mainPageViewModelRef.Projects.Count % mainPageViewModelRef.ColorList.Count];
            UpdateDurationText();
        }

        public Project(MainPageViewModel mainPageViewModelRef, Response<ProjectItem> response) : this(
            response.Data.Name, mainPageViewModelRef, response.Data.Id)
        {
            
        }

        public TimeSpan TotalDuraction()
        {
            TimeSpan res = new TimeSpan();
            res = res.Add(Duration);
            foreach (var variable in _tasks)
            {
                if (variable.IsStarted && variable.lastTimes()!=null)
                {
                    res = res.Add(DateTime.Now - variable.lastTimes()!.StartDate);
                }
                res = res.Add(variable.Duration);
            }
            return res;
        }
        
        public override ICommand Remove{ get { return new Command(async () =>
        {
            IsEdited = false;
            //IsStarted = false;
            MainPageViewModelRef.RemoveAllTasks(this.Tasks);
            MainPageViewModelRef.Projects.Remove(this);
            await ApiSingleton.Instance.Api.deleteprojetAsync(ApiSingleton.Instance.access_token, projectId);
        }); } }

        public void OpenTaskView()
        {
            ((NavigationPage) Application.Current.MainPage).PushAsync(new ProjectView(this), true); 
        }

        public override ICommand Tapped => new Command(()=>OpenTaskView());

        public void StartStopCommand()
        {
            if (StartedObj != null && StartedObj.IsStarted)
            {
                StartedObj.IsStarted = false;
                return;
            }
            OpenTaskView();
        }
        public override ICommand StartOrStopCommand
        {
            get
            {
                return new Command(StartStopCommand);
            }
        }

        public override void UpdateDurationText()
        {
            Debug.WriteLine(StartedObj?.IsStarted == true);
            if (StartedObj != null)
            {
                StartStopText = StartedObj.IsStarted ? "Stop" : "Start";
                DurationColor = StartedObj.IsStarted ? HighlightColor : DefaultColor;
            }
            TimeSpan totalDuration = TotalDuraction(); 
            DurationText = (totalDuration.Hours > 0 ? totalDuration.Hours+"h " : "") + totalDuration.Minutes + "min";
        }

    }

}