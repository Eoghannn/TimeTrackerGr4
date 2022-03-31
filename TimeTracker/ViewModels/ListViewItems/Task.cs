using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TimeTracker.API;
using TimeTracker.API.Projects;
using TimeTracker.Model;
using Xamarin.Forms;

namespace TimeTracker.ViewModels.ListViewItems
{
    public sealed class Task : ProjectTask
    {
        private List<Times> TimesList;

        public void addTimes(Times t)
        {
            Duration = Duration.Add(t.Duration());
            TimesList.Add(t);
        }

        public Times? lastTimes()
        {
            if (TimesList.Count == 0)
            {
                return null;
            }
            else
            {
                return TimesList[TimesList.Count - 1];
            }
        }
        public Project Project { get; set; }
        
        public override bool IsEdited { 
            get
            {
                return base.IsEdited;
            }
            set
            {
                if (!value)
                {
                    // le titre de la task a potentiellement été modifié : informer le serveur ici
                    ApiSingleton.Instance.Api.modiftaskAsync(ApiSingleton.Instance.access_token, Project.projectId, taskId, Title).ContinueWith((
                        async task =>
                        {
                            Response<TaskItem> response = await task;
                            // TODO : vérifier erreur
                        } ));
                }

                base.IsEdited = value;
            } 
        }

        public long taskId;
        
        public Boolean IsStarted
        {
            get => base.IsStarted;
            set
            {
                Project.MainPageViewModelRef.LastTask = this;
                Task srtdObj = Project.StartedObj;
                if (value)
                {
                    Project.StartedObj = this;
                    if (srtdObj != null && srtdObj != this)
                    {
                        srtdObj.IsStarted = false;
                    }
                    foreach (var VARIABLE in TimesList) // dans le doute on vérifie que tous les anciens "times" ont bien été arrété avant d'en mettre un nouveau
                    {
                        VARIABLE.End();
                    }

                    ApiSingleton.Instance.Api.settimetaskAsync(ApiSingleton.Instance.access_token, Project.projectId,
                        taskId, DateTime.Now, DateTime.Now).ContinueWith((async task =>
                    {
                        Response<TimeItem> response = await task;
                        // TODO : vérifier erreur
                        Times t = new Times(this, response.Data.Id); 
                        TimesList.Add(t);
                        Device.StartTimer(new TimeSpan(0, 0, 10), () =>
                        {
                            UpdateDurationText();
                            return base.IsStarted;
                        });
                    }));
                    
                    
                }
                else
                {
                    if (TimesList.Count > 0)
                    {
                        TimesList[TimesList.Count - 1].End();
                        Duration = Duration.Add(TimesList[TimesList.Count - 1].Duration());
                    }
                    
                }
                Project.IsStarted =  Project.StartedObj?.IsStarted ?? false;
                base.IsStarted = value;
                UpdateDurationText();
            }
        }

        public Task(String title, Project project, long taskId): base(project.MainPageViewModelRef)
        {
            Title = title;
            Project = project;
            this.taskId = taskId;
            if (project.MainPageViewModelRef.ColorList.Count == 0) Color = Color.White;
            else Color = Project.MainPageViewModelRef.ColorList[project.Tasks.Count % Project.MainPageViewModelRef.ColorList.Count];
            TimesList = new List<Times>();
            project.MainPageViewModelRef.LastTask = this;
            UpdateDurationText();
        }

        public override ICommand Tapped { get; }

        public override ICommand Remove{ get { return new Command(async () =>
        {
            IsEdited = false;
            IsStarted = false;
            Project.Tasks.Remove(this);
            if (Project.MainPageViewModelRef.LastTask == this)
            {
                Project.MainPageViewModelRef.LastTask = null;
            }
            await ApiSingleton.Instance.Api.deletetaskAsync(ApiSingleton.Instance.access_token, Project.projectId, taskId);
        }); } }

        public override void UpdateDurationText()
        {
            StartStopText = IsStarted ? "Stop" : "Start";
            DurationColor = IsStarted ? HighlightColor : DefaultColor;
            
            TimeSpan totalDuration = Duration;
            if(IsStarted && TimesList.Count>0) totalDuration = totalDuration.Add(DateTime.Now - TimesList[TimesList.Count-1].StartDate);
            DurationText = (totalDuration.Hours > 0 ? totalDuration.Hours+"h " : "") + totalDuration.Minutes + "min";
            Project.UpdateDurationText();
        }

        public override ICommand StartOrStopCommand
        {
            get { return new Command(() => { IsStarted = !IsStarted; }); }
        }
    }
}