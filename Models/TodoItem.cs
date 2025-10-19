using System;

using System.ComponentModel.DataAnnotations.Schema;

namespace UniPlanner.Models
{
    /// <summary>
    /// Represents a personal to-do item
    /// Inherits from BaseItem to demonstrate polymorphism
    /// </summary>
    [Table("Todos")]
    public class TodoItem : BaseItem
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public string Category { get; set; }  // Personal, Study, Health, etc.

        public TodoItem()
        {
            IsCompleted = false;
            Category = "Personal";
        }

        public TodoItem(string title) : this()
        {
            Title = title;
        }

        public override string ToString()
        {
            return Title;
        }

        /// <summary>
        /// Implementation of abstract method from BaseItem
        /// </summary>
        public override string GetDisplayInfo()
        {
            string status = IsCompleted ? "✓" : "☐";
            return $"{status} {Title} [{Category}]";
        }

        /// <summary>
        /// Implementation of abstract validation method
        /// </summary>
        public override bool Validate()
        {
            return !string.IsNullOrWhiteSpace(Title) &&
                   Title.Length >= 1 && Title.Length <= 200 &&
                   !string.IsNullOrWhiteSpace(Category);
        }

        /// <summary>
        /// Override virtual method to provide specific entity type
        /// </summary>
        public override string GetEntityType()
        {
            return "Personal To-Do";
        }
    }
}

