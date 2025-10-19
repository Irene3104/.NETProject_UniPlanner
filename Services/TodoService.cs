using System;
using System.Collections.Generic;
using System.Linq;
using UniPlanner.Models;
using UniPlanner.Data;

namespace UniPlanner.Services
{
    /// <summary>
    /// Personal to-do list management service
    /// NOW USING ENTITY FRAMEWORK for database operations
    /// </summary>
    public class TodoService : IRepository<TodoItem>
    {

        /// <summary>
        /// Add new todo item using Entity Framework
        /// </summary>
        public void Add(TodoItem item)
        {
            using (var context = new UniPlannerContext())
            {
                context.Todos.Add(item);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update existing todo item using Entity Framework
        /// </summary>
        public void Update(TodoItem item)
        {
            using (var context = new UniPlannerContext())
            {
                var existing = context.Todos.Find(item.Id);
                if (existing != null)
                {
                    existing.Title = item.Title;
                    existing.IsCompleted = item.IsCompleted;
                    existing.Category = item.Category;
                    existing.UpdateModifiedDate();
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete todo item using Entity Framework
        /// </summary>
        public void Delete(int id)
        {
            using (var context = new UniPlannerContext())
            {
                var item = context.Todos.Find(id);
                if (item != null)
                {
                    context.Todos.Remove(item);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Get todo item by ID using Entity Framework
        /// </summary>
        public TodoItem GetById(int id)
        {
            using (var context = new UniPlannerContext())
            {
                return context.Todos.Find(id);
            }
        }

        /// <summary>
        /// Get all todo items using Entity Framework with LINQ
        /// </summary>
        public IReadOnlyList<TodoItem> GetAll()
        {
            using (var context = new UniPlannerContext())
            {
                return context.Todos
                    .OrderBy(t => t.IsCompleted)
                    .ThenByDescending(t => t.CreatedDate)
                    .ToList();
            }
        }

        /// <summary>
        /// Toggle todo completion status using Entity Framework
        /// </summary>
        public void ToggleComplete(int id)
        {
            using (var context = new UniPlannerContext())
            {
                var item = context.Todos.Find(id);
                if (item != null)
                {
                    item.IsCompleted = !item.IsCompleted;
                    item.UpdateModifiedDate();
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Get active (incomplete) todos using Entity Framework with LINQ
        /// </summary>
        public IReadOnlyList<TodoItem> GetActive()
        {
            using (var context = new UniPlannerContext())
            {
                return context.Todos
                    .Where(t => !t.IsCompleted)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToList();
            }
        }

        /// <summary>
        /// Get completed todos using Entity Framework with LINQ
        /// </summary>
        public IReadOnlyList<TodoItem> GetCompleted()
        {
            using (var context = new UniPlannerContext())
            {
                return context.Todos
                    .Where(t => t.IsCompleted)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToList();
            }
        }
    }
}

