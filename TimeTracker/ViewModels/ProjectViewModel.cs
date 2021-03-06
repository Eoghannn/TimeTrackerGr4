using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microcharts;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using Storm.Mvvm;
using TimeTracker.API;
using TimeTracker.API.Projects;
using TimeTracker.ViewModels.ListViewItems;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        private String _title;
        private readonly ProjectView parent;
        private ObservableCollection<Task> _tasks;

        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
        }

        public String Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ProjectViewModel(ProjectView p)
        {
            this.parent = p;
            Title = parent.project.Title;
            _tasks = parent.project.Tasks;
        }

        public ICommand NewTask
        {
            get
            {
                return new Command(async () =>
                {
                    Response<TaskItem> response = await 
                        ApiSingleton.Instance.Api.createtaskAsync(ApiSingleton.Instance.access_token,
                            parent.project.projectId, "Nouvelle tâche");
                    // TODO : vérifier erreur
                    Task t = new Task(response.Data.Name, parent.project, response.Data.Id);
                    Tasks.Add(t);
                    t.IsEdited = true;
                });
            }
        }
        
        public List<ChartEntry> BuildProjectEntries(SKColor textColor, int max_label_length)
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            foreach (var task in Tasks)
            {
                ChartEntry chartEntry = new ChartEntry(task.Duration.Minutes); 
                chartEntry.Color = SKColor.Parse(task.Color.ToHex());
                if (task.Title.Length > max_label_length) chartEntry.Label = task.Title.Substring(0, max_label_length-3) + "...";
                else chartEntry.Label = task.Title;
                chartEntry.TextColor = textColor;
                chartEntry.ValueLabelColor = textColor;
                entries.Add(chartEntry);
            }
            
            return entries;
        }
        
        public ICommand ShowGraph => new Command(async () => await Application.Current.MainPage.Navigation.PushPopupAsync(new GraphPopUp(BuildProjectEntries(SKColors.White, 20))));
            
    }
}
