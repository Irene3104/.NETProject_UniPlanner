using System;

namespace UniPlanner.Models
{
    /// <summary>
    /// Represents a personal to-do item
    /// </summary>
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public string Category { get; set; }  // Personal, Study, Health, etc.
        public DateTime CreatedDate { get; set; }

        public TodoItem()
        {
            IsCompleted = false;
            CreatedDate = DateTime.Now;
            Category = "Personal";
        }

        public TodoItem(string title)
        {
            Title = title;
            IsCompleted = false;
            CreatedDate = DateTime.Now;
            Category = "Personal";
        }

        public override string ToString()
        {
            return Title;
        }
    }
}

