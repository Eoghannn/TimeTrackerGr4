using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace TimeTracker
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }
    
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged( 
            [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke( 
                this, new PropertyChangedEventArgs( propertyName ) );
        }

        protected void SetValue<T>( ref T backingField, T value, [CallerMemberName] string propertyName = null )
        {
            if (EqualityComparer<T>.Default.Equals( 
                    backingField, value )) return;
            backingField = value;
            OnPropertyChanged( propertyName );
        }
    }

    public class ObjWithDuration: BaseViewModel
    {
        
        private DateTime _startTime;

        private Color _highlightColor;
        private Color _defaultColor;
        private Color _actualColor;

        public Color DurationColor
        {
            get => _actualColor;
            set => SetValue(ref _actualColor, value);
        }

        private bool _isStarted;
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                if (value)
                {
                    DurationColor = _highlightColor;
                    _startTime = DateTime.Now;
                    Device.StartTimer(new TimeSpan(0, 0, 10), () =>
                    {
                        UpdateDurationText();
                        return _isStarted;
                    });
                }
                else
                {
                    DurationColor = _defaultColor;
                    Duration = Duration.Add(DateTime.Now - _startTime);
                }
                StartStopText = value ? "Stop" : "Start";
                SetValue<bool>(ref _isStarted, value);
            }
        }

        private String _startStopText;
        public String StartStopText
        {
            get => _startStopText;
            set => SetValue<String>( ref _startStopText, value );
        }
        
        private String _durationText;
        public String DurationText
        {
            get => _durationText;
            set => SetValue<String>( ref _durationText, value );
        }

        private TimeSpan _duration;

        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                SetValue(ref _duration, value);
                // TODO mettre à jour le serveur ici ( sur la durée de la task / project ) avec tempTS
            }
        }

        public ObjWithDuration()
        {
            _startTime = new DateTime();
            _isStarted = false;
            _startStopText = "Start";
            _duration = new TimeSpan();
            _highlightColor = (Color) (Application.Current.Resources.ContainsKey("Highlight")
                ? Application.Current.Resources["Highlight"]
                : Color.Aqua);
            _defaultColor = (Color) (Application.Current.Resources.ContainsKey("Primary")
                ? Application.Current.Resources["Primary"]
                : Color.White);
            _actualColor = _defaultColor;
            UpdateDurationText();
        }

        public void UpdateDurationText()
        {
            // Fonction appelée toutes les 10 secondes quand le timer est lancé 
            Debug.WriteLine("update ");
            TimeSpan tempTS = new TimeSpan(Duration.Days, Duration.Hours, Duration.Minutes, Duration.Seconds);
            if (IsStarted)
            {
                tempTS = tempTS.Add(DateTime.Now - _startTime);
            }
            DurationText = (tempTS.Hours > 0 ? tempTS.Hours+"h " : "") + tempTS.Minutes + "min";
        }
        
        
        public ICommand StartOrStopCommand { get { return new Command(() => {IsStarted = !IsStarted; }); } } 
        
    }
    public class Task : ObjWithDuration
    {
        private String _title;
        private Project _parent;

        public Task(String title, Project parent)
        {
            this._title = title;
            this._parent = parent;
        }
    }

    public class Project : ObjWithDuration
    {
        private static Project _inEdition; // il ne peut y avoir qu'un project en édition à la fois

        public static Project inEdition
        {
            get => _inEdition;
            set
            {
                if (value == null) MainPage.nPButton.IsEnabled = true;
                else MainPage.nPButton.IsEnabled = false;
                _inEdition = value;
            }
        }
        private String _title;
        public String Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }
        private ObservableCollection<Task> _tasks;

        private bool _isEdited;
        
        private Entry EditEntry
        {
            get
            {
                ITemplatedItemsList<Cell> cells = MainPage.PListView.TemplatedItems;
                int index = MainPage.Projects.IndexOf(this);
                return index==-1? null : cells?[index].FindByName<Entry>("EditEntry");
            }
        }

        public bool IsEdited
        {
            get => _isEdited;
            set
            {
                if (!value)
                {
                    if (inEdition == this)
                    {
                        inEdition = null;
                    }
                    // TODO ici l'utilisateur a potentiellement changer le titre du projet, si c'est le cas il faut l'envoyer au serveur
                }
                else
                {
                    if (inEdition != null)
                    {
                        inEdition.IsEdited = false;
                    }
                    inEdition = this;
                    if (EditEntry != null)
                    {
                        Debug.WriteLine("focus");
                        Debug.WriteLine(EditEntry.Text);
                        EditEntry.Focus();
                        EditEntry.Unfocused += (sender, args) => IsEdited = false ;
                    }
                }
                
                SetValue(ref _isEdited, value);
            }
        }

        public Project(string title)
        {
            Title = title;
            _tasks = new ObservableCollection<Task>();
        }
        
        public TimeSpan TotalDuraction()
        {
            TimeSpan res = new TimeSpan();
            res = res.Add(Duration);
            foreach (var variable in _tasks)
            {
                res = res.Add(variable.Duration);
            }
            return res;
        }
        public ICommand ToggleEdit { get { return new Command(() => {IsEdited = !IsEdited; }); } }
        public ICommand RemoveProject { get { return new Command(() =>
        {
            IsEdited = false;
            MainPage.Projects.Remove(this);
            // TODO notifier le serveur de la suppression de ce projet
        }); } }
    }

    public partial class MainPage : ContentPage
    {

        public static ObservableCollection<Project> Projects;

        public static ListView PListView;
        public static Button nPButton;
        public MainPage()
        {
            Projects = new ObservableCollection<Project>();
            InitializeComponent();
            PListView = projectListView;
            nPButton = newProjectButton;
            Projects.Add(new Project("Projet Xamarin"));
            Projects.Add(new Project("Devoir Outils Exploration Données"));
            Projects.Add(new Project("Projet Flutter"));
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new GraphPopUp());
        }

        private void New_Project(object sender, EventArgs e)
        {
            Project p = new Project("Nouveau Projet");
            Projects.Add(p);
            // /!\ ne pas éditer avant d'ajouter à la liste sinon pb
            p.IsEdited = true;
        }
        
        
    }
}
