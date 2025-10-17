using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UniPlanner.Models;

namespace UniPlanner.Services
{
    /// <summary>
    /// File I/O service for JSON import/export
    /// </summary>
    public class FileService
    {
        private readonly TaskService _taskService;
        private readonly ScheduleService _scheduleService;

        public FileService()
        {
            _taskService = new TaskService();
            _scheduleService = new ScheduleService();
        }

        /// <summary>
        /// Export all tasks to JSON file
        /// </summary>
        public void ExportTasks(string filePath)
        {
            try
            {
                var tasks = _taskService.GetAll();
                var json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to export tasks: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Import tasks from JSON file
        /// </summary>
        public void ImportTasks(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("File not found", filePath);

                var json = File.ReadAllText(filePath);
                var tasks = JsonConvert.DeserializeObject<List<TaskItem>>(json);

                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        task.Id = 0; // Reset ID for new insertion
                        _taskService.Add(task);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to import tasks: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Export schedule to JSON file
        /// </summary>
        public void ExportSchedule(string filePath)
        {
            try
            {
                var schedule = _scheduleService.GetAll();
                var json = JsonConvert.SerializeObject(schedule, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to export schedule: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Import schedule from JSON file
        /// </summary>
        public void ImportSchedule(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("File not found", filePath);

                var json = File.ReadAllText(filePath);
                var schedules = JsonConvert.DeserializeObject<List<ScheduleItem>>(json);

                if (schedules != null)
                {
                    foreach (var schedule in schedules)
                    {
                        schedule.Id = 0; // Reset ID for new insertion
                        _scheduleService.Add(schedule);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to import schedule: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Export all data (tasks + schedule)
        /// </summary>
        public void ExportAllData(string filePath)
        {
            try
            {
                var data = new
                {
                    Tasks = _taskService.GetAll(),
                    Schedule = _scheduleService.GetAll(),
                    ExportDate = DateTime.Now
                };

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to export data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Create backup of database
        /// </summary>
        public void CreateBackup(string backupPath)
        {
            try
            {
                var dbPath = "Data\\uni.db";
                if (File.Exists(dbPath))
                {
                    File.Copy(dbPath, backupPath, true);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create backup: {ex.Message}", ex);
            }
        }
    }
}

