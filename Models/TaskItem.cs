using System;

namespace UniPlanner.Models
{
    /// <summary>
    /// Represents a task or assignment with deadline tracking
    /// </summary>
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; } // High, Medium, Low
        public bool IsCompleted { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        public TaskItem()
        {
            Priority = "Medium";
            IsCompleted = false;
            DueDate = DateTime.Today;
        }

        public TaskItem(string title, DateTime dueDate, string priority = "Medium")
        {
            Title = title;
            DueDate = dueDate;
            Priority = priority;
            IsCompleted = false;
        }

        /// <summary>
        /// Check if task is overdue
        /// </summary>
        public bool IsOverdue()
        {
            return !IsCompleted && DueDate < DateTime.Today;
        }

        /// <summary>
        /// Get days remaining until due date
        /// </summary>
        public int DaysRemaining()
        {
            return (DueDate - DateTime.Today).Days;
        }

        public override string ToString()
        {
            return $"{Title} - Due: {DueDate:yyyy-MM-dd} [{Priority}]";
        }
    }
}

