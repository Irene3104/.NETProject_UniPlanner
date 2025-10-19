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
    /// Schedule management service with SQLite + Dapper
    /// </summary>
    public class ScheduleService : IRepository<ScheduleItem>
    {
        private string ConnectionString => 
            ConfigurationManager.ConnectionStrings["UniDb"]?.ConnectionString 
            ?? "Data Source=Data\\uni.db;Version=3;foreign keys=true;";

        /// <summary>
        /// Add new schedule item
        /// </summary>
        public void Add(ScheduleItem item)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                var subjectService = new SubjectService();
                var subject = subjectService.UpsertFromSchedule(item.SubjectCode, item.SubjectName, item.Instructor);

                if (subject != null)
                {
                    item.SubjectCode = subject.Code;
                    item.SubjectName = subject.Name;

                    if (string.IsNullOrWhiteSpace(item.Instructor))
                    {
                        item.Instructor = subject.Instructor;
                    }
                }

                conn.Execute(
                    @"INSERT INTO Schedule(DayOfWeek, SubjectCode, SubjectName, StartTime, EndTime, Location, Instructor) 
                      VALUES(@DayOfWeek, @SubjectCode, @SubjectName, @StartTime, @EndTime, @Location, @Instructor)",
                    new
                    {
                        item.DayOfWeek,
                        item.SubjectCode,
                        item.SubjectName,
                        item.StartTime,
                        item.EndTime,
                        item.Location,
                        item.Instructor
                    }
                );
            }
        }

        /// <summary>
        /// Update existing schedule item
        /// </summary>
        public void Update(ScheduleItem item)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                var subjectService = new SubjectService();
                var subject = subjectService.UpsertFromSchedule(item.SubjectCode, item.SubjectName, item.Instructor);

                if (subject != null)
                {
                    item.SubjectCode = subject.Code;
                    item.SubjectName = subject.Name;

                    if (string.IsNullOrWhiteSpace(item.Instructor))
                    {
                        item.Instructor = subject.Instructor;
                    }
                }

                conn.Execute(
                    @"UPDATE Schedule 
                      SET DayOfWeek = @DayOfWeek, SubjectCode = @SubjectCode, SubjectName = @SubjectName,
                          StartTime = @StartTime, EndTime = @EndTime, 
                          Location = @Location, Instructor = @Instructor
                      WHERE Id = @Id",
                    new
                    {
                        item.Id,
                        item.DayOfWeek,
                        item.SubjectCode,
                        item.SubjectName,
                        item.StartTime,
                        item.EndTime,
                        item.Location,
                        item.Instructor
                    }
                );
            }
        }

        /// <summary>
        /// Delete schedule item by ID
        /// </summary>
        public void Delete(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Execute("DELETE FROM Schedule WHERE Id = @Id", new { Id = id });
            }
        }

        /// <summary>
        /// Get schedule item by ID
        /// </summary>
        public ScheduleItem GetById(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                return conn.QueryFirstOrDefault<ScheduleItem>(
                    "SELECT * FROM Schedule WHERE Id = @Id", 
                    new { Id = id }
                );
            }
        }

        /// <summary>
        /// Get all schedule items
        /// </summary>
        public IReadOnlyList<ScheduleItem> GetAll()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                return conn.Query<ScheduleItem>("SELECT * FROM Schedule").ToList();
            }
        }

        /// <summary>
        /// Get schedule for specific day (LINQ demonstration)
        /// </summary>
        public IReadOnlyList<ScheduleItem> GetByDay(int dayOfWeek)
        {
            return GetAll()
                .Where(s => s.DayOfWeek == dayOfWeek)
                .OrderBy(s => s.StartTime)
                .ToList();
        }

        /// <summary>
        /// Get all schedules ordered by day and time (LINQ demonstration)
        /// </summary>
        public IReadOnlyList<ScheduleItem> GetAllOrdered()
        {
            return GetAll()
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToList();
        }

        /// <summary>
        /// Get schedule by subject (LINQ demonstration)
        /// </summary>
        public IReadOnlyList<ScheduleItem> GetBySubject(string subject)
        {
            return GetAll()
                .Where(s => s.SubjectCode.Contains(subject))
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToList();
        }

        /// <summary>
        /// Get total weekly study hours (LINQ demonstration)
        /// </summary>
        public double GetTotalWeeklyHours()
        {
            return GetAll()
                .Sum(s => s.GetDurationHours());
        }

        /// <summary>
        /// Get schedule grouped by day (LINQ demonstration)
        /// </summary>
        public Dictionary<int, List<ScheduleItem>> GetGroupedByDay()
        {
            return GetAll()
                .GroupBy(s => s.DayOfWeek)
                .ToDictionary(g => g.Key, g => g.OrderBy(s => s.StartTime).ToList());
        }

        /// <summary>
        /// Check if a schedule conflicts with existing schedules
        /// </summary>
        public bool HasTimeConflict(ScheduleItem newSchedule, int excludeId = 0)
        {
            if (!TimeSpan.TryParse(newSchedule.StartTime, out var newStart) ||
                !TimeSpan.TryParse(newSchedule.EndTime, out var newEnd))
            {
                return false; // Invalid time format
            }

            var existingSchedules = GetByDay(newSchedule.DayOfWeek)
                .Where(s => s.Id != excludeId); // Exclude current item when updating

            foreach (var existing in existingSchedules)
            {
                if (!TimeSpan.TryParse(existing.StartTime, out var existingStart) ||
                    !TimeSpan.TryParse(existing.EndTime, out var existingEnd))
                {
                    continue;
                }

                // Check if times overlap
                // Conflict occurs if:
                // 1. New start time is between existing start and end
                // 2. New end time is between existing start and end
                // 3. New schedule completely encompasses existing schedule
                if ((newStart >= existingStart && newStart < existingEnd) ||
                    (newEnd > existingStart && newEnd <= existingEnd) ||
                    (newStart <= existingStart && newEnd >= existingEnd))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get conflicting schedule details for user-friendly message
        /// </summary>
        public ScheduleItem GetConflictingSchedule(ScheduleItem newSchedule, int excludeId = 0)
        {
            if (!TimeSpan.TryParse(newSchedule.StartTime, out var newStart) ||
                !TimeSpan.TryParse(newSchedule.EndTime, out var newEnd))
            {
                return null;
            }

            var existingSchedules = GetByDay(newSchedule.DayOfWeek)
                .Where(s => s.Id != excludeId);

            foreach (var existing in existingSchedules)
            {
                if (!TimeSpan.TryParse(existing.StartTime, out var existingStart) ||
                    !TimeSpan.TryParse(existing.EndTime, out var existingEnd))
                {
                    continue;
                }

                if ((newStart >= existingStart && newStart < existingEnd) ||
                    (newEnd > existingStart && newEnd <= existingEnd) ||
                    (newStart <= existingStart && newEnd >= existingEnd))
                {
                    return existing;
                }
            }

            return null;
        }
    }
}

