using System.Data.Entity;
using UniPlanner.Models;

namespace UniPlanner.Data
{
    /// <summary>
    /// Entity Framework DbContext for UniPlanner
    /// Provides ORM capabilities for database operations
    /// </summary>
    public class UniPlannerContext : DbContext
    {
        public UniPlannerContext() : base("name=UniDb")
        {
            Database.SetInitializer<UniPlannerContext>(null);
        }

        // DbSets for each entity
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<ScheduleItem> Schedules { get; set; }
        public DbSet<TodoItem> Todos { get; set; }
        public DbSet<SubjectItem> Subjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure TaskItem
            modelBuilder.Entity<TaskItem>().ToTable("Tasks");
            modelBuilder.Entity<TaskItem>().HasKey(t => t.Id);
            modelBuilder.Entity<TaskItem>().Property(t => t.Title).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<TaskItem>().Property(t => t.DueDate).IsRequired();
            modelBuilder.Entity<TaskItem>().Property(t => t.Priority).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<TaskItem>().Property(t => t.IsCompleted).IsRequired();
            modelBuilder.Entity<TaskItem>().Property(t => t.SubjectCode).HasMaxLength(50);
            modelBuilder.Entity<TaskItem>().Property(t => t.SubjectName).HasMaxLength(100);
            modelBuilder.Entity<TaskItem>().Property(t => t.Description).HasMaxLength(1000);

            // Configure ScheduleItem
            modelBuilder.Entity<ScheduleItem>().ToTable("Schedule");
            modelBuilder.Entity<ScheduleItem>().HasKey(s => s.Id);
            modelBuilder.Entity<ScheduleItem>().Property(s => s.DayOfWeek).IsRequired();
            modelBuilder.Entity<ScheduleItem>().Property(s => s.SubjectCode).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<ScheduleItem>().Property(s => s.SubjectName).HasMaxLength(100);
            modelBuilder.Entity<ScheduleItem>().Property(s => s.StartTime).IsRequired().HasMaxLength(5);
            modelBuilder.Entity<ScheduleItem>().Property(s => s.EndTime).IsRequired().HasMaxLength(5);
            modelBuilder.Entity<ScheduleItem>().Property(s => s.Location).HasMaxLength(100);
            modelBuilder.Entity<ScheduleItem>().Property(s => s.Instructor).HasMaxLength(100);

            // Configure TodoItem
            modelBuilder.Entity<TodoItem>().ToTable("Todos");
            modelBuilder.Entity<TodoItem>().HasKey(t => t.Id);
            modelBuilder.Entity<TodoItem>().Property(t => t.Title).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<TodoItem>().Property(t => t.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.Category).HasMaxLength(50);
            modelBuilder.Entity<TodoItem>().Property(t => t.CreatedDate).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.ModifiedDate);

            base.OnModelCreating(modelBuilder);
        }
    }
}

