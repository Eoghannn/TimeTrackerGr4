using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public abstract class ObjWithDuration: EditableObject
    {
        public List<Times> TimesList;
        private Color _highlightColor;
        private Color _defaultColor;
        private Color _actualColor;

        public Color DurationColor
        {
            get => _actualColor;
            set => SetValue(ref _actualColor, value);
        }

        protected Project _parent { get; set; }

        protected bool _isStarted;
        public virtual bool IsStarted
        {
            get => _isStarted;
            set
            {
                SetValue<bool>(ref _isStarted, value);
                UpdateDurationText();
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
            TimesList = new List<Times>();
            _isStarted = false;
            _startStopText = "Start";
            _duration = new TimeSpan();
            _highlightColor = App.HighlightColor;
            _defaultColor = App.DefaultColor;
            _actualColor = _defaultColor;
            UpdateDurationText();
        }
        public void UpdateDurationText()
        {
            // Fonction appelée toutes les 10 secondes quand le timer est lancé 
            Debug.WriteLine("update ");
            DurationColor = IsStarted ? _highlightColor : _defaultColor;
            StartStopText = IsStarted ? "Stop" : "Start";
            TimeSpan totalDuration = Duration;
            if (this.GetType() == typeof(Project))
            {
                ObjWithDuration startedObj = ((Project) this).StartedObj;
                if (startedObj != null)
                {
                    StartStopText = startedObj.IsStarted ? "Stop" : "Start";
                    DurationColor = startedObj.IsStarted ? _highlightColor : _defaultColor;
                }
                if(((Project) this).Tasks != null) totalDuration = ((Project) this).TotalDuraction(); // null au démarrage car le constructeur de ObjWith duration est appelé avant 
            }
            else if (IsStarted)
            {
                totalDuration = totalDuration.Add(DateTime.Now - TimesList[TimesList.Count-1].StartDate);
            }
            DurationText = (totalDuration.Hours > 0 ? totalDuration.Hours+"h " : "") + totalDuration.Minutes + "min";
            if (_parent != null)
            {
                _parent.UpdateDurationText();
            }
        }
        public virtual ICommand StartOrStopCommand { get { return new Command(() => {IsStarted = !IsStarted; }); } } 
        
    }
}