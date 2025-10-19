using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using UniPlanner.Models;
using System;

namespace UniPlanner.Services
{
    /// <summary>
    /// Subject/Course management service
    /// </summary>
    public class SubjectService : IRepository<SubjectItem>
    {
        private string ConnectionString => 
            ConfigurationManager.ConnectionStrings["UniDb"]?.ConnectionString 
            ?? "Data Source=Data\\uni.db;Version=3;foreign keys=true;";

        /// <summary>
        /// Add new subject
        /// </summary>
        public void Add(SubjectItem item)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Execute(
                    @"INSERT INTO Subjects(Code, Name, Instructor, Credits, Color) 
                      VALUES(@Code, @Name, @Instructor, @Credits, @Color)",
                    item
                );
            }
        }

        /// <summary>
        /// Update existing subject
        /// </summary>
        public void Update(SubjectItem item)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Execute(
                    @"UPDATE Subjects 
                      SET Code = @Code, Name = @Name, Instructor = @Instructor, 
                          Credits = @Credits, Color = @Color
                      WHERE Id = @Id",
                    item
                );
            }
        }

        /// <summary>
        /// Delete subject by ID and cascade delete related schedules and tasks
        /// </summary>
        public void Delete(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Get subject code before deletion
                        var subject = conn.QueryFirstOrDefault<SubjectItem>(
                            "SELECT * FROM Subjects WHERE Id = @Id", 
                            new { Id = id });
                        
                        if (subject != null)
                        {
                            // Delete related schedules
                            conn.Execute(
                                "DELETE FROM Schedule WHERE SubjectCode = @SubjectCode", 
                                new { SubjectCode = subject.Code });
                            
                            // Delete related tasks
                            conn.Execute(
                                "DELETE FROM Tasks WHERE Subject = @SubjectCode", 
                                new { SubjectCode = subject.Code });
                            
                            // Delete the subject itself
                            conn.Execute(
                                "DELETE FROM Subjects WHERE Id = @Id", 
                                new { Id = id });
                        }
                        
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Get subject by ID
        /// </summary>
        public SubjectItem GetById(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                return conn.QueryFirstOrDefault<SubjectItem>(
                    "SELECT * FROM Subjects WHERE Id = @Id", 
                    new { Id = id }
                );
            }
        }

        /// <summary>
        /// Get all subjects
        /// </summary>
        public IReadOnlyList<SubjectItem> GetAll()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                return conn.Query<SubjectItem>("SELECT * FROM Subjects ORDER BY Code").ToList();
            }
        }

        /// <summary>
        /// Get subject by code
        /// </summary>
        public SubjectItem GetByCode(string code)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                return conn.QueryFirstOrDefault<SubjectItem>(
                    "SELECT * FROM Subjects WHERE Code = @Code", 
                    new { Code = code }
                );
            }
        }

        /// <summary>
        /// Check if subject code exists
        /// </summary>
        public bool CodeExists(string code, int excludeId = 0)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                var count = conn.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Subjects WHERE Code = @Code AND Id != @ExcludeId",
                    new { Code = code, ExcludeId = excludeId }
                );
                return count > 0;
            }
        }

        /// <summary>
        /// Initialize with sample subjects if database is empty
        /// </summary>
        public void InitializeSampleData()
        {
            if (GetAll().Count == 0)
            {
                var samples = new[]
                {
                    new SubjectItem("COMP101", "Introduction to Programming") { Credits = 3, Color = "#3498db" },
                    new SubjectItem("MATH201", "Calculus I") { Credits = 4, Color = "#e74c3c" },
                    new SubjectItem("PHYS101", "Physics I") { Credits = 4, Color = "#2ecc71" },
                    new SubjectItem("ENGL102", "Academic Writing") { Credits = 3, Color = "#f39c12" },
                    new SubjectItem("COMP102", "Data Structures") { Credits = 3, Color = "#9b59b6" },
                };

                foreach (var subject in samples)
                {
                    Add(subject);
                }
            }
        }

        public SubjectItem UpsertFromSchedule(string code, string name, string instructor)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            code = code.Trim();
            string proposedName = string.IsNullOrWhiteSpace(name) ? code : name.Trim();
            string proposedInstructor = string.IsNullOrWhiteSpace(instructor) ? null : instructor.Trim();

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                var existing = conn.QueryFirstOrDefault<SubjectItem>(
                    "SELECT * FROM Subjects WHERE Code = @Code",
                    new { Code = code });

                if (existing == null)
                {
                    var item = new SubjectItem
                    {
                        Code = code,
                        Name = proposedName,
                        Instructor = proposedInstructor,
                        Credits = 0,
                        Color = GenerateColorForCode(code)
                    };

                    conn.Execute(
                        "INSERT INTO Subjects(Code, Name, Instructor, Credits, Color) VALUES(@Code, @Name, @Instructor, @Credits, @Color)",
                        item);

                    return conn.QueryFirstOrDefault<SubjectItem>(
                        "SELECT * FROM Subjects WHERE Code = @Code",
                        new { Code = code });
                }

                bool requiresUpdate = false;

                if (!string.IsNullOrWhiteSpace(proposedName) && !string.Equals(existing.Name, proposedName, StringComparison.Ordinal))
                {
                    existing.Name = proposedName;
                    requiresUpdate = true;
                }

                if (!string.IsNullOrWhiteSpace(proposedInstructor) && !string.Equals(existing.Instructor, proposedInstructor, StringComparison.Ordinal))
                {
                    existing.Instructor = proposedInstructor;
                    requiresUpdate = true;
                }

                if (string.IsNullOrWhiteSpace(existing.Color))
                {
                    existing.Color = GenerateColorForCode(code);
                    requiresUpdate = true;
                }

                if (requiresUpdate)
                {
                    conn.Execute(
                        "UPDATE Subjects SET Name = @Name, Instructor = @Instructor, Color = @Color WHERE Id = @Id",
                        existing);
                }

                return existing;
            }
        }

        private string GenerateColorForCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return "#A3D5FF";
            }

            var palette = new[]
            {
                "#A3D5FF", "#BDE4A8", "#FFECB3", "#FFCDD2", "#D1C4E9",
                "#F8BBD0", "#C8E6C9", "#FFE0B2", "#BBDEFB", "#E1BEE7"
            };

            int index = Math.Abs(code.GetHashCode()) % palette.Length;
            return palette[index];
        }
    }
}

