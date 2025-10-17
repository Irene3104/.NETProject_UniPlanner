using System;
using System.Text.RegularExpressions;

namespace UniPlanner.Utils
{
    /// <summary>
    /// Input validation helper methods
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Validate that a string is not empty
        /// </summary>
        public static bool IsValidString(string value, int minLength = 1, int maxLength = 500)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            
            return value.Trim().Length >= minLength && value.Trim().Length <= maxLength;
        }

        /// <summary>
        /// Validate time format (HH:mm)
        /// </summary>
        public static bool IsValidTimeFormat(string time)
        {
            if (string.IsNullOrWhiteSpace(time))
                return false;
            
            return TimeSpan.TryParse(time, out _);
        }

        /// <summary>
        /// Validate that end time is after start time
        /// </summary>
        public static bool IsValidTimeRange(string startTime, string endTime)
        {
            if (!IsValidTimeFormat(startTime) || !IsValidTimeFormat(endTime))
                return false;
            
            var start = TimeSpan.Parse(startTime);
            var end = TimeSpan.Parse(endTime);
            
            return end > start;
        }

        /// <summary>
        /// Validate date is not in the past
        /// </summary>
        public static bool IsValidFutureDate(DateTime date, bool allowToday = true)
        {
            if (allowToday)
                return date.Date >= DateTime.Today;
            
            return date.Date > DateTime.Today;
        }

        /// <summary>
        /// Validate priority value
        /// </summary>
        public static bool IsValidPriority(string priority)
        {
            if (string.IsNullOrWhiteSpace(priority))
                return false;
            
            var validPriorities = new[] { "High", "Medium", "Low" };
            return Array.Exists(validPriorities, p => p.Equals(priority, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validate day of week (0-6)
        /// </summary>
        public static bool IsValidDayOfWeek(int day)
        {
            return day >= 0 && day <= 6;
        }

        /// <summary>
        /// Validate numeric range
        /// </summary>
        public static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Get validation error message for common scenarios
        /// </summary>
        public static string GetErrorMessage(string fieldName, string validationType)
        {
            switch (validationType.ToLower())
            {
                case "required":
                    return $"{fieldName} is required.";
                case "invalid":
                    return $"{fieldName} is invalid.";
                case "toolong":
                    return $"{fieldName} is too long.";
                case "tooshort":
                    return $"{fieldName} is too short.";
                case "pastdate":
                    return $"{fieldName} cannot be in the past.";
                default:
                    return $"{fieldName} validation failed.";
            }
        }
    }
}

