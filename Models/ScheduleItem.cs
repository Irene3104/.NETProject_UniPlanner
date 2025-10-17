using System;

namespace UniPlanner.Models
{
    /// <summary>
    /// Represents a class schedule entry
    /// </summary>
    public class ScheduleItem
    {
        public int Id { get; set; }
        public int DayOfWeek { get; set; } // 0=Sunday, 1=Monday, ..., 6=Saturday
        public string Subject { get; set; }
        public string SubjectName { get; set; }
        public string StartTime { get; set; } // Format: HH:mm
        public string EndTime { get; set; }   // Format: HH:mm
        public string Location { get; set; }
        public string Instructor { get; set; }

        public ScheduleItem()
        {
            DayOfWeek = 1; // Monday
            StartTime = "09:00";
            EndTime = "10:00";
        }

        public ScheduleItem(int dayOfWeek, string subject, string startTime, string endTime)
        {
            DayOfWeek = dayOfWeek;
            Subject = subject;
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// Get day name from day of week number
        /// </summary>
        public string GetDayName()
        {
            return ((DayOfWeek)DayOfWeek).ToString();
        }

        /// <summary>
        /// Calculate duration in hours
        /// </summary>
        public double GetDurationHours()
        {
            if (TimeSpan.TryParse(StartTime, out var start) && TimeSpan.TryParse(EndTime, out var end))
            {
                return (end - start).TotalHours;
            }
            return 0;
        }

        public override string ToString()
        {
            return $"{GetDayName()} {StartTime}-{EndTime}: {Subject}";
        }
    }
}

