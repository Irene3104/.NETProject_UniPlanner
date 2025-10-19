using System;

using System.ComponentModel.DataAnnotations.Schema;

namespace UniPlanner.Models
{
    /// <summary>
    /// Represents a class schedule entry
    /// Inherits from BaseItem to demonstrate polymorphism
    /// </summary>
    [Table("Schedule")]
    public class ScheduleItem : BaseItem
    {
        public int DayOfWeek { get; set; } // 0=Sunday, 1=Monday, ..., 6=Saturday
        [Column("SubjectCode")]
        public string SubjectCode { get; set; }
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

        public ScheduleItem(int dayOfWeek, string subjectCode, string startTime, string endTime)
        {
            DayOfWeek = dayOfWeek;
            SubjectCode = subjectCode;
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
            return $"{GetDayName()} {StartTime}-{EndTime}: {SubjectCode}";
        }

        /// <summary>
        /// Implementation of abstract method from BaseItem
        /// </summary>
        public override string GetDisplayInfo()
        {
            string subjectDisplay = !string.IsNullOrEmpty(SubjectName) ? SubjectName : SubjectCode;
            string locationInfo = !string.IsNullOrEmpty(Location) ? $" @ {Location}" : "";
            return $"{GetDayName()} {StartTime}-{EndTime}: {subjectDisplay}{locationInfo}";
        }

        /// <summary>
        /// Implementation of abstract validation method
        /// </summary>
        public override bool Validate()
        {
            return DayOfWeek >= 0 && DayOfWeek <= 6 &&
                   !string.IsNullOrWhiteSpace(SubjectCode) &&
                   !string.IsNullOrWhiteSpace(StartTime) &&
                   !string.IsNullOrWhiteSpace(EndTime) &&
                   TimeSpan.TryParse(StartTime, out var start) &&
                   TimeSpan.TryParse(EndTime, out var end) &&
                   end > start;
        }

        /// <summary>
        /// Override virtual method to provide specific entity type
        /// </summary>
        public override string GetEntityType()
        {
            return "Class Schedule";
        }
    }
}

