using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker.ViewModels.ListViewItems
{
    public abstract class ProjectTask : EditableObject
    {
        /* PARTIE COMMUNE ENTRE PROJECT ET PROJECT TASK */

        protected Color HighlightColor;
        protected Color DefaultColor;
        protected Color ActualColor;
        public Color DurationColor
        {
            get => ActualColor;
            set => SetProperty(ref ActualColor, value);
        }
        
        private bool _isStarted;
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                SetProperty<bool>(ref _isStarted, value);
                UpdateDurationText();
            }
        }
        
        private String _startStopText;
        public String StartStopText
        {
            get => _startStopText;
            set => SetProperty<String>( ref _startStopText, value );
        }
        
        private String _durationText;
        public String DurationText
        {
            get => _durationText;
            set => SetProperty<String>( ref _durationText, value );
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                SetProperty(ref _duration, value);
            }
        }
        
        private String _title;
        public String Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        
        private Color _color;
        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        
        public abstract void UpdateDurationText();
        public abstract ICommand StartOrStopCommand { get;  }
        public abstract ICommand Tapped { get; }
        public abstract ICommand Remove { get; }

        public ProjectTask(MainPageViewModel mainPageViewModel): base(mainPageViewModel)
        {
            _isStarted = false;
            _startStopText = "Start";
            _duration = new TimeSpan();
            HighlightColor = App.HighlightColor;
            DefaultColor = App.DefaultColor;
            ActualColor = DefaultColor;
            
        }
    }
}