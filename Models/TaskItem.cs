using System;

using System.ComponentModel.DataAnnotations.Schema;

namespace UniPlanner.Models
{
    /// <summary>
    /// Represents a task or assignment with deadline tracking
    /// Inherits from BaseItem to demonstrate polymorphism
    /// </summary>
    [Table("Tasks")]
    public class TaskItem : BaseItem
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; } // High, Medium, Low
        public bool IsCompleted { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; } // Store subject name directly
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
            return $"{Title} - Due: {DueDate:dd-MM-yyyy} [{Priority}]";
        }

        /// <summary>
        /// Implementation of abstract method from BaseItem
        /// </summary>
        public override string GetDisplayInfo()
        {
            string status = IsCompleted ? "✓ Completed" : "⏳ Pending";
            return $"{Title} - Due: {DueDate:dd-MM-yyyy} | Priority: {Priority} | Status: {status}";
        }

        /// <summary>
        /// Implementation of abstract validation method
        /// </summary>
        public override bool Validate()
        {
            return !string.IsNullOrWhiteSpace(Title) &&
                   Title.Length >= 1 && Title.Length <= 200 &&
                   !string.IsNullOrWhiteSpace(Priority) &&
                   (Priority == "High" || Priority == "Medium" || Priority == "Low");
        }

        /// <summary>
        /// Override virtual method to provide specific entity type
        /// </summary>
        public override string GetEntityType()
        {
            return "Assignment/Task";
        }
    }
}

