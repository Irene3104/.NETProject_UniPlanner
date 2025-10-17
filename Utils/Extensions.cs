using System;

namespace UniPlanner.Utils
{
    /// <summary>
    /// Extension methods for common types
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Get friendly date string (e.g., "Today", "Tomorrow", "In 3 days")
        /// </summary>
        public static string ToFriendlyDate(this DateTime date)
        {
            var days = (date.Date - DateTime.Today).Days;
            
            if (days == 0) return "Today";
            if (days == 1) return "Tomorrow";
            if (days == -1) return "Yesterday";
            if (days > 1 && days <= 7) return $"In {days} days";
            if (days < -1 && days >= -7) return $"{Math.Abs(days)} days ago";
            
            return date.ToString("MMM dd, yyyy");
        }

        /// <summary>
        /// Check if date is within current week
        /// </summary>
        public static bool IsThisWeek(this DateTime date)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);
            
            return date >= startOfWeek && date < endOfWeek;
        }

        /// <summary>
        /// Truncate string to specified length with ellipsis
        /// </summary>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
        }

        /// <summary>
        /// Check if string is null, empty, or whitespace
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Get week number of the year
        /// </summary>
        public static int GetWeekNumber(this DateTime date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(date, 
                System.Globalization.CalendarWeekRule.FirstDay, 
                DayOfWeek.Sunday);
        }

        /// <summary>
        /// Format time span to readable string
        /// </summary>
        public static string ToReadableString(this TimeSpan span)
        {
            if (span.TotalMinutes < 60)
                return $"{(int)span.TotalMinutes} min";
            
            if (span.TotalHours < 24)
                return $"{span.Hours}h {span.Minutes}m";
            
            return $"{(int)span.TotalDays} days";
        }
    }
}

