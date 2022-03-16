using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
        public class Project : ProjectTask
    {
        private ObservableCollection<Task> _tasks;
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
        
        public override Entry EditEntry
        {
            // propriété surchargée pour permettre à l'editable object de connaitre l'entry sur laquel set le focus lors d'un changement d'état ( éditable à non éditable )
            get
            {
                ITemplatedItemsList<Cell> cells = MainPage.PListView.TemplatedItems;
                int index = MainPage.Projects.IndexOf(this);
                return index==-1? null : cells?[index].FindByName<Entry>("EditEntry");
            }
        }
        
        public Project(string title)
        {
            Title = title;
            _tasks = new ObservableCollection<Task>();
            if (MainPage.ColorList.Count == 0) Color = Color.White;
            else Color = MainPage.ColorList[MainPage.Projects.Count % MainPage.ColorList.Count];
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
            Task.RemoveAll(this.Tasks);
            MainPage.Projects.Remove(this);
            // TODO notifier le serveur de la suppression de ce projet
        }); } }

        public void OpenTaskView()
        {
            ((NavigationPage) App.Current.MainPage).PushAsync(new ProjectView(this), true); 
        }

        public override ICommand Tapped => new Command(()=>OpenTaskView());

        public void StartStopCommand()
        {
            if (IsStarted)
            {
                if (StartedObj != null)
                {
                    StartedObj.IsStarted = false;
                }
            }
            else
            {
                OpenTaskView();
            }
        }
        public override ICommand StartOrStopCommand
        {
            get
            {
                return new Command(StartStopCommand);
            }
        }

    }

}