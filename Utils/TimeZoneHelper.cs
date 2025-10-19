using System;

namespace UniPlanner.Utils
{
    /// <summary>
    /// Helper class for Sydney timezone operations
    /// </summary>
    public static class TimeZoneHelper
    {
        private static TimeZoneInfo _sydneyTimeZone;

        /// <summary>
        /// Get Sydney timezone
        /// </summary>
        public static TimeZoneInfo GetSydneyTimeZone()
        {
            if (_sydneyTimeZone == null)
            {
                try
                {
                    // Try Windows timezone ID first
                    _sydneyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
                }
                catch (TimeZoneNotFoundException)
                {
                    try
                    {
                        // Try IANA timezone ID
                        _sydneyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");
                    }
                    catch (TimeZoneNotFoundException)
                    {
                        // Fallback to local timezone
                        _sydneyTimeZone = TimeZoneInfo.Local;
                    }
                }
            }
            return _sydneyTimeZone;
        }

        /// <summary>
        /// Convert UTC time to Sydney time
        /// </summary>
        public static DateTime ConvertToSydney(DateTime utcTime)
        {
            var sydneyTz = GetSydneyTimeZone();
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, sydneyTz);
        }

        /// <summary>
        /// Convert local time to Sydney time
        /// </summary>
        public static DateTime ConvertToSydney(DateTime localTime, TimeZoneInfo sourceTimeZone = null)
        {
            var sydneyTz = GetSydneyTimeZone();
            
            if (sourceTimeZone == null)
            {
                sourceTimeZone = localTime.Kind == DateTimeKind.Utc
                    ? TimeZoneInfo.Utc
                    : TimeZoneInfo.Local;
            }

            if (localTime.Kind == DateTimeKind.Utc)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(localTime, sydneyTz);
            }

            // Always work with an unspecified DateTime so ConvertTime uses the provided source timezone
            var unspecifiedTime = DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified);

            if (sourceTimeZone.Id == sydneyTz.Id)
            {
                return unspecifiedTime;
            }

            return TimeZoneInfo.ConvertTime(unspecifiedTime, sourceTimeZone, sydneyTz);
        }

        /// <summary>
        /// Get current Sydney time
        /// </summary>
        public static DateTime GetSydneyNow()
        {
            return ConvertToSydney(DateTime.UtcNow);
        }

        /// <summary>
        /// Get Sydney timezone display name
        /// </summary>
        public static string GetSydneyTimeZoneDisplayName()
        {
            var sydneyTz = GetSydneyTimeZone();
            return sydneyTz.DisplayName;
        }

        /// <summary>
        /// Check if a date is overdue in Sydney timezone
        /// </summary>
        public static bool IsOverdue(DateTime dueDate, TimeZoneInfo sourceTimeZone = null)
        {
            var sydneyNow = GetSydneyNow();
            var sydneyDue = ConvertToSydney(dueDate, sourceTimeZone);
            return sydneyDue.Date < sydneyNow.Date;
        }

        /// <summary>
        /// Format relative time display for Sydney timezone
        /// </summary>
        public static string FormatRelativeTime(DateTime dateTime, TimeZoneInfo sourceTimeZone = null, bool assumeSydneyTime = false)
        {
            var sydneyNow = GetSydneyNow();
            var sydneyDateTime = assumeSydneyTime
                ? DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified)
                : ConvertToSydney(dateTime, sourceTimeZone);
            var days = (sydneyDateTime.Date - sydneyNow.Date).Days;
            
            string relative;
            if (days == 0) relative = "Today";
            else if (days == 1) relative = "Tomorrow";
            else if (days == -1) relative = "Yesterday";
            else if (days > 1 && days <= 7) relative = $"In {days} days";
            else if (days < -1 && days >= -7) relative = $"{Math.Abs(days)} days ago";
            else relative = sydneyDateTime.ToString("dd/MM/yyyy");

            if (relative == sydneyDateTime.ToString("dd/MM/yyyy"))
            {
                return relative;
            }

            return $"{sydneyDateTime:dd/MM/yyyy} ({relative})";
        }
    }
}
