using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker.ViewModels.ListViewItems
{
        public sealed class Project : ProjectTask
    {
        private ObservableCollection<Task> _tasks;
        public readonly MainPageViewModel MainPageViewModelRef;
        public Task StartedObj{ get; set; }


        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set => SetValue(ref _tasks, value);
        }

        public void AddTask(String title)
        {
            AddTask(new Task(title, this));
        }
        public void AddTask(Task t)
        {
            Tasks.Add(t);
        }
        
        
        public Project(string title, MainPageViewModel mainPageViewModelRef) : base(mainPageViewModelRef)
        {
            Title = title;
            MainPageViewModelRef = mainPageViewModelRef;
            _tasks = new ObservableCollection<Task>();
            if (mainPageViewModelRef.ColorList.Count == 0) Color = Color.White;
            else Color = mainPageViewModelRef.ColorList[mainPageViewModelRef.Projects.Count % mainPageViewModelRef.ColorList.Count];
            UpdateDurationText();
        }
        public TimeSpan TotalDuraction()
        {
            TimeSpan res = new TimeSpan();
            res = res.Add(Duration);
            foreach (var variable in _tasks)
            {
                if (variable.IsStarted)
                {
                    res = res.Add(DateTime.Now - variable.TimesList[variable.TimesList.Count - 1].StartDate);
                }
                res = res.Add(variable.Duration);
            }
            return res;
        }
        
        public override ICommand Remove{ get { return new Command(() =>
        {
            IsEdited = false;
            //IsStarted = false;
            MainPageViewModelRef.RemoveAllTasks(this.Tasks);
            MainPageViewModelRef.Projects.Remove(this);
            // TODO notifier le serveur de la suppression de ce projet
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