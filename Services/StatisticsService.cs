using System;
using System.Collections.Generic;
using System.Linq;
using UniPlanner.Models;

namespace UniPlanner.Services
{
    /// <summary>
    /// Statistics and analytics service with LINQ aggregations
    /// </summary>
    public class StatisticsService
    {
        private readonly TaskService _taskService;
        private readonly ScheduleService _scheduleService;

        public StatisticsService()
        {
            _taskService = new TaskService();
            _scheduleService = new ScheduleService();
        }

        /// <summary>
        /// Calculate task completion percentage
        /// </summary>
        public double GetCompletionRate()
        {
            var tasks = _taskService.GetAll();
            if (tasks.Count == 0) return 0;

            var completed = tasks.Count(t => t.IsCompleted);
            return (double)completed / tasks.Count * 100;
        }

        /// <summary>
        /// Get task count by priority (LINQ demonstration)
        /// </summary>
        public Dictionary<string, int> GetTaskCountByPriority()
        {
            return _taskService.GetAll()
                .GroupBy(t => t.Priority)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Get task count by status
        /// </summary>
        public Dictionary<string, int> GetTaskCountByStatus()
        {
            var tasks = _taskService.GetAll();
            
            return new Dictionary<string, int>
            {
                { "Completed", tasks.Count(t => t.IsCompleted) },
                { "Pending", tasks.Count(t => !t.IsCompleted && t.DueDate >= DateTime.Today) },
                { "Overdue", tasks.Count(t => t.IsOverdue()) }
            };
        }

        /// <summary>
        /// Get upcoming tasks for next 7 days (LINQ demonstration)
        /// </summary>
        public IReadOnlyList<TaskItem> GetUpcomingTasks(int days = 7)
        {
            var endDate = DateTime.Today.AddDays(days);
            
            return _taskService.GetAll()
                .Where(t => !t.IsCompleted && t.DueDate >= DateTime.Today && t.DueDate <= endDate)
                .OrderBy(t => t.DueDate)
                .ToList();
        }

        /// <summary>
        /// Get study schedule summary
        /// </summary>
        public Dictionary<string, object> GetScheduleSummary()
        {
            var schedules = _scheduleService.GetAll();
            
            return new Dictionary<string, object>
            {
                { "TotalClasses", schedules.Count },
                { "TotalWeeklyHours", _scheduleService.GetTotalWeeklyHours() },
                { "UniqueSubjects", schedules.Select(s => s.Subject).Distinct().Count() },
                { "BusiestDay", GetBusiestDay() }
            };
        }

        /// <summary>
        /// Get busiest day of week (LINQ demonstration)
        /// </summary>
        public string GetBusiestDay()
        {
            var grouped = _scheduleService.GetAll()
                .GroupBy(s => s.DayOfWeek)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            if (grouped == null) return "None";
            
            return ((DayOfWeek)grouped.Day).ToString();
        }

        /// <summary>
        /// Get productivity metrics
        /// </summary>
        public Dictionary<string, object> GetProductivityMetrics()
        {
            var tasks = _taskService.GetAll();
            var completedTasks = tasks.Where(t => t.IsCompleted).ToList();
            
            return new Dictionary<string, object>
            {
                { "TotalTasks", tasks.Count },
                { "CompletedTasks", completedTasks.Count },
                { "PendingTasks", tasks.Count(t => !t.IsCompleted) },
                { "OverdueTasks", tasks.Count(t => t.IsOverdue()) },
                { "CompletionRate", GetCompletionRate() },
                { "AverageTasksPerWeek", CalculateAverageTasksPerWeek(tasks) }
            };
        }

        /// <summary>
        /// Calculate average tasks per week (LINQ demonstration)
        /// </summary>
        private double CalculateAverageTasksPerWeek(IReadOnlyList<TaskItem> tasks)
        {
            if (tasks.Count == 0) return 0;

            var weeks = tasks
                .GroupBy(t => GetWeekNumber(t.DueDate))
                .Count();

            return weeks > 0 ? (double)tasks.Count / weeks : 0;
        }

        /// <summary>
        /// Get week number of year
        /// </summary>
        private int GetWeekNumber(DateTime date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(date, 
                System.Globalization.CalendarWeekRule.FirstDay, 
                DayOfWeek.Sunday);
        }

        /// <summary>
        /// Get tasks due today
        /// </summary>
        public IReadOnlyList<TaskItem> GetTasksDueToday()
        {
            return _taskService.GetAll()
                .Where(t => t.DueDate.Date == DateTime.Today && !t.IsCompleted)
                .OrderBy(t => t.Priority == "High" ? 1 : t.Priority == "Medium" ? 2 : 3)
                .ToList();
        }
    }
}

