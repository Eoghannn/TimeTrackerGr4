using System;
using TimeTracker.API;
using TimeTracker.API.Projects;
using TimeTracker.ViewModels.ListViewItems;

namespace TimeTracker.Model
{
    public class Times
    {
        public DateTime StartDate;
        public DateTime? EndDate;

        public Task Task;

        public long id; 
        public Times(Task task, long id)
        {
            StartDate = DateTime.Now;
            EndDate = null;
            this.Task = task;
            this.id = id;
        }

        public Times(Task task, long id, DateTime startDate, DateTime endDate)
        {
            this.Task = task;
            this.id = id;
            EndDate = endDate;
            StartDate = startDate;
        }
        public void End()
        {
            if (EndDate == null)
            {
                EndDate = DateTime.Now;
                ApiSingleton.Instance.Api.modiftimetaskAsync(ApiSingleton.Instance.access_token, Task.Project.projectId,
                    Task.taskId, id, StartDate, DateTime.Now).ContinueWith((async task =>
                {
                    Response<TimeItem> response = await task;
                    // TODO : vérifier erreur
                }));
            }
        }

        public TimeSpan Duration()
        {
            return (EndDate ?? DateTime.Now) - StartDate;
        }
    }
}