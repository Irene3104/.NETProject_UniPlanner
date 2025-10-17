using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using UniPlanner.Models;

namespace UniPlanner.Services
{
    /// <summary>
    /// Personal to-do list management service
    /// </summary>
    public class TodoService : IRepository<TodoItem>
    {
        private string ConnectionString => 
            ConfigurationManager.ConnectionStrings["UniDb"]?.ConnectionString 
            ?? "Data Source=Data\\uni.db;Version=3;foreign keys=true;";

        public void Add(TodoItem item)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Execute(
                    @"INSERT INTO Todos(Title, IsCompleted, Category, CreatedDate) 
                      VALUES(@Title, @IsCompleted, @Category, @CreatedDate)",
                    new
                    {
                        item.Title,
                        IsCompleted = item.IsCompleted ? 1 : 0,
                        item.Category,
                        CreatedDate = item.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
                    }
                );
            }
        }

        public void Update(TodoItem item)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Execute(
                    @"UPDATE Todos 
                      SET Title = @Title, IsCompleted = @IsCompleted, Category = @Category
                      WHERE Id = @Id",
                    new
                    {
                        item.Id,
                        item.Title,
                        IsCompleted = item.IsCompleted ? 1 : 0,
                        item.Category
                    }
                );
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Execute("DELETE FROM Todos WHERE Id = @Id", new { Id = id });
            }
        }

        public TodoItem GetById(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                var result = conn.QueryFirstOrDefault<dynamic>(
                    "SELECT * FROM Todos WHERE Id = @Id", 
                    new { Id = id }
                );
                return result != null ? MapToTodoItem(result) : null;
            }
        }

        public IReadOnlyList<TodoItem> GetAll()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                var results = conn.Query<dynamic>("SELECT * FROM Todos ORDER BY IsCompleted, CreatedDate DESC");
                return results.Select(MapToTodoItem).ToList();
            }
        }

        public void ToggleComplete(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Execute(
                    "UPDATE Todos SET IsCompleted = NOT IsCompleted WHERE Id = @Id", 
                    new { Id = id }
                );
            }
        }

        public IReadOnlyList<TodoItem> GetActive()
        {
            return GetAll().Where(t => !t.IsCompleted).ToList();
        }

        public IReadOnlyList<TodoItem> GetCompleted()
        {
            return GetAll().Where(t => t.IsCompleted).ToList();
        }

        private TodoItem MapToTodoItem(dynamic row)
        {
            return new TodoItem
            {
                Id = (int)(long)row.Id,
                Title = row.Title,
                IsCompleted = row.IsCompleted == 1,
                Category = row.Category,
                CreatedDate = DateTime.Parse(row.CreatedDate)
            };
        }
    }
}

