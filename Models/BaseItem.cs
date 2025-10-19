using System;

namespace UniPlanner.Models
{
    /// <summary>
    /// Abstract base class for all domain entities
    /// Demonstrates polymorphism through inheritance and virtual methods
    /// </summary>
    public abstract class BaseItem
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        protected BaseItem()
        {
            CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// Abstract method - must be implemented by derived classes
        /// Returns display information for UI
        /// </summary>
        public abstract string GetDisplayInfo();

        /// <summary>
        /// Abstract method - validates entity data
        /// </summary>
        public abstract bool Validate();

        /// <summary>
        /// Virtual method - can be overridden by derived classes
        /// Returns entity type name
        /// </summary>
        public virtual string GetEntityType()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Virtual method - returns formatted created date
        /// </summary>
        public virtual string GetFormattedCreatedDate()
        {
            return CreatedDate.ToString("dd-MM-yyyy HH:mm");
        }

        /// <summary>
        /// Update modification timestamp
        /// </summary>
        public void UpdateModifiedDate()
        {
            ModifiedDate = DateTime.Now;
        }

        /// <summary>
        /// Check if entity was recently created (within last 24 hours)
        /// </summary>
        public bool IsNew()
        {
            return (DateTime.Now - CreatedDate).TotalHours < 24;
        }
    }
}

