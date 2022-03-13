using System;

namespace TimeTracker.ViewModels
{
    public class Times : BaseViewModel
    {
        private DateTime _startDate;
        private DateTime _endDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetValue(ref _startDate, value);
        }
        public DateTime EndDate
        {
            get => _endDate;
            set => SetValue(ref _endDate, value);
        }
        public Times()
        {
            _startDate = DateTime.Now;
            MainPage.RefreshLastTask();
        }

        public Times(DateTime startDate, DateTime endDate)
        {
            _endDate = endDate;
            _startDate = startDate;
        }
        public void End()
        {
            _endDate = DateTime.Now;
            MainPage.RefreshLastTask();
        }

        public TimeSpan Duration()
        {
            return _endDate - _startDate;
        }
    }
}