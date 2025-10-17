using System;

namespace UniPlanner.Models
{
    /// <summary>
    /// Represents a subject/course
    /// </summary>
    public class SubjectItem
    {
        public int Id { get; set; }
        public string Code { get; set; }        // e.g., COMP101
        public string Name { get; set; }        // e.g., Introduction to Programming
        public string Instructor { get; set; }
        public int Credits { get; set; }
        public string Color { get; set; }       // For UI color coding

        public SubjectItem()
        {
            Credits = 3;
            Color = "#3498db";
        }

        public SubjectItem(string code, string name)
        {
            Code = code;
            Name = name;
            Credits = 3;
            Color = "#3498db";
        }

        /// <summary>
        /// Get display text for dropdowns (as property for ComboBox binding)
        /// </summary>
        public string GetDisplayText => $"{Code} - {Name}";

        /// <summary>
        /// Override ToString for ComboBox display
        /// </summary>
        public override string ToString()
        {
            return GetDisplayText;
        }
    }
}

