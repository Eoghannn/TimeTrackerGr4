using System;

namespace TimeTracker.Model
{
    public class Times
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public Times()
        {
            StartDate = DateTime.Now;
        }

        public Times(DateTime startDate, DateTime endDate)
        {
            EndDate = endDate;
            StartDate = startDate;
        }
        public void End()
        {
            EndDate = DateTime.Now;
        }

        public TimeSpan Duration()
        {
            return EndDate - StartDate;
        }
    }
}