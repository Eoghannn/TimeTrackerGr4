using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class Task : ProjectTask
    {
        private static List<Task> alltasks = new List<Task>();

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
            ((Project)_parent).Tasks.Remove(this);
            // TODO notifier le serveur de la suppression de cette task
        }); } }
        
        public override Entry EditEntry
        {
            // propriété surchargée pour permettre à l'editable object de connaitre l'entry sur laquel set le focus lors d'un changement d'état ( éditable à non éditable )
            get
            {
                ITemplatedItemsList<Cell> cells = ProjectView.currentTaskListView.TemplatedItems;
                int index = ((Project)_parent).Tasks.IndexOf(this);
                return index==-1? null : cells?[index].FindByName<Entry>("EditEntry");
            }
        }
    }
}