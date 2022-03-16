using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class Task : ProjectTask
    {
        private static List<Task> alltasks;

        public static void RemoveAll(Collection<Task> tasks)
        {
            foreach (var task in tasks)
            {
                alltasks.Remove(task);
            }
            MainPage.RefreshLastTask();
        }

        public static void init()
        {
            alltasks = new List<Task>();
        }
        public static Task LastTask()
        {
            Task last = null;
            Times lastTimes = null;
            foreach (var task in alltasks)
            {
                List<Times> tl = task.TimesList;
                if (last == null)
                {
                    last = task;
                    if (tl.Count > 0)
                    {
                        lastTimes = tl[tl.Count - 1];
                    }
                }
                if (tl.Count == 0)
                {
                    continue;
                }
                Times lastT = tl[tl.Count - 1];
                if (lastTimes == null || lastT.StartDate > lastTimes?.StartDate)
                {
                    last = task;
                    lastTimes = lastT;
                }
            }

            return last;
        }

        public override Boolean IsStarted
        {
            get => _isStarted;
            set
            {
                Task srtdObj = _parent.StartedObj;
                if (value)
                {
                    _parent.StartedObj = this;
                    if (srtdObj != null && srtdObj != this)
                    {
                        srtdObj.IsStarted = false;
                    }
                    foreach (var VARIABLE in
                             TimesList) // dans le doute on vérifie que tous les anciens "times" ont bien été arrété avant d'en mettre un nouveau
                    {
                        VARIABLE.End();
                    }

                    TimesList.Add(new Times());
                    Device.StartTimer(new TimeSpan(0, 0, 10), () =>
                    {
                        UpdateDurationText();
                        return _isStarted;
                    });
                }
                else
                {
                    TimesList[TimesList.Count - 1].End();
                    Duration = Duration.Add(TimesList[TimesList.Count - 1].Duration());
                }
                _parent.IsStarted =  _parent.StartedObj?.IsStarted ?? false;
                SetValue<bool>(ref _isStarted, value);
                UpdateDurationText();
            }
        }

        public Task(String title, Project parent)
        {
            Title = title;
            _parent = parent;
            if (alltasks.Count == 0) Color = Color.White;
            else Color = MainPage.ColorList[alltasks.Count % MainPage.ColorList.Count];
            TimesList = new List<Times>();
            alltasks.Add(this);
        }
        
        public override ICommand Remove{ get { return new Command(() =>
        {
            IsEdited = false;
            IsStarted = false;
            ((Project)_parent).Tasks.Remove(this);
            // TODO notifier le serveur de la suppression de cette task
        }); } }
        
        public override Entry EditEntry
        {
            // propriété surchargée pour permettre à l'editable object de connaitre l'entry sur laquel set le focus lors d'un changement d'état ( éditable à non éditable )
            get
            {
                if (ProjectView.currentTaskListView == null)
                {
                    // last task
                    return MainPage.LTListView.TemplatedItems[0].FindByName<Entry>("EditEntry");
                }
                ITemplatedItemsList<Cell> cells = ProjectView.currentTaskListView.TemplatedItems;
                int index = ((Project)_parent).Tasks.IndexOf(this);
                return index==-1? new Entry() : cells?[index].FindByName<Entry>("EditEntry");
            }
        }
    }
}