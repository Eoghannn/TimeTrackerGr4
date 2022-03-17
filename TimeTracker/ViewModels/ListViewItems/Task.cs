using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TimeTracker.Model;
using Xamarin.Forms;

namespace TimeTracker.ViewModels.ListViewItems
{
    public sealed class Task : ProjectTask
    {
        public List<Times> TimesList;
        private Project _parent { get; set; }
        
        public Boolean IsStarted
        {
            get => base.IsStarted;
            set
            {
                _parent.MainPageViewModelRef.LastTask = this;
                Task srtdObj = _parent.StartedObj;
                if (value)
                {
                    _parent.StartedObj = this;
                    if (srtdObj != null && srtdObj != this)
                    {
                        srtdObj.IsStarted = false;
                    }
                    foreach (var VARIABLE in TimesList) // dans le doute on vérifie que tous les anciens "times" ont bien été arrété avant d'en mettre un nouveau
                    {
                        VARIABLE.End();
                    }

                    TimesList.Add(new Times());
                    Device.StartTimer(new TimeSpan(0, 0, 10), () =>
                    {
                        UpdateDurationText();
                        return base.IsStarted;
                    });
                }
                else
                {
                    TimesList[TimesList.Count - 1].End();
                    Duration = Duration.Add(TimesList[TimesList.Count - 1].Duration());
                }
                _parent.IsStarted =  _parent.StartedObj?.IsStarted ?? false;
                base.IsStarted = value;
                UpdateDurationText();
            }
        }

        public Task(String title, Project parent): base(parent.MainPageViewModelRef)
        {
            Title = title;
            _parent = parent;
            if (parent.MainPageViewModelRef.ColorList.Count == 0) Color = Color.White;
            else Color = _parent.MainPageViewModelRef.ColorList[parent.Tasks.Count % _parent.MainPageViewModelRef.ColorList.Count];
            TimesList = new List<Times>();
            parent.MainPageViewModelRef.LastTask = this;
            UpdateDurationText();
        }

        public override ICommand Tapped { get; }

        public override ICommand Remove{ get { return new Command(() =>
        {
            IsEdited = false;
            IsStarted = false;
            _parent.Tasks.Remove(this);
            // TODO notifier le serveur de la suppression de cette task
        }); } }

        public override void UpdateDurationText()
        {
            StartStopText = IsStarted ? "Stop" : "Start";
            DurationColor = IsStarted ? HighlightColor : DefaultColor;            
            TimeSpan totalDuration = Duration;
            if(IsStarted) totalDuration = totalDuration.Add(DateTime.Now - TimesList[TimesList.Count-1].StartDate);
            DurationText = (totalDuration.Hours > 0 ? totalDuration.Hours+"h " : "") + totalDuration.Minutes + "min";
            _parent.UpdateDurationText();
        }

        public override ICommand StartOrStopCommand
        {
            get { return new Command(() => { IsStarted = !IsStarted; }); }
        }
    }
}