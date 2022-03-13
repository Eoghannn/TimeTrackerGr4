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

        protected ObjWithDuration _parent { get; set; }
        protected int startedItems = 0;


        private bool _isStarted;
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                if (value)
                {
                    DurationColor = _highlightColor;
                    if (_parent != null)
                    {
                        _parent.startedItems += 1;
                        if (_parent.startedItems > 0)
                        {
                            _parent.DurationColor = _highlightColor;
                        }
                    }
                    foreach (var VARIABLE in TimesList) // dans le doute on vérifie que tous les anciens "times" ont bien été arrété avant d'en mettre un nouveau
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
                    if (_parent != null)
                    {
                        _parent.startedItems -= 1;
                        if (_parent.startedItems == 0)
                        {
                            _parent.DurationColor = _defaultColor;
                        }
                    }
                    if (startedItems == 0)
                    {
                        DurationColor = _defaultColor;
                    }
                    TimesList[TimesList.Count-1].End();
                    Duration = Duration.Add(TimesList[TimesList.Count-1].Duration());
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
            TimeSpan totalDuration = Duration;
            if (this.GetType() == typeof(Project))
            {
                if(((Project) this).Tasks != null) totalDuration = ((Project) this).TotalDuraction(); // null au démarrage car le constructeur de ObjWith duration est appelé avant 
            }
            TimeSpan tempTS = new TimeSpan(totalDuration.Days, totalDuration.Hours, totalDuration.Minutes, totalDuration.Seconds);
            if (IsStarted)
            {
                tempTS = tempTS.Add(DateTime.Now - TimesList[TimesList.Count-1].StartDate);
            }
            DurationText = (tempTS.Hours > 0 ? tempTS.Hours+"h " : "") + tempTS.Minutes + "min";
            if (_parent != null)
            {
                _parent.UpdateDurationText();
            }
        }
        public ICommand StartOrStopCommand { get { return new Command(() => {IsStarted = !IsStarted; }); } } 
        
    }
}