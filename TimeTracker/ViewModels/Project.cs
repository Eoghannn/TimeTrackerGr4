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
            // TODO pb du cas ou plusieurs tâches ont été effectué en même temps ( est-ce qu'on affiche 2 min car on a passé 1 min sur 2 tâche ? )
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
            MainPage.Projects.Remove(this);
            // TODO notifier le serveur de la suppression de ce projet
        }); } }

        public override ICommand Tapped
        {
            get
            {
                return new Command(async () =>
                {
                    await ((NavigationPage) App.Current.MainPage).PushAsync(new ProjectView(this), true);
                });
            }
        }
    }

}