using System;
using System.Configuration;
using System.Data.SQLite;
using System.IO;

namespace UniPlanner.Services
{
    /// <summary>
    /// Database initialization and schema creation
    /// </summary>
    public static class DbBootstrap
    {
        private static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["UniDb"]?.ConnectionString
            ?? "Data Source=|DataDirectory|\\uni.db;Version=3;foreign keys=true;";

        /// <summary>
        /// Ensure database and tables are created
        /// </summary>
        public static void EnsureCreated()
        {
            try
            {
                // Get database path and ensure directory exists
                var dbPath = GetDatabasePath();
                var directory = Path.GetDirectoryName(dbPath);
                
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Check if this is a new database
                bool isNewDatabase = !File.Exists(dbPath);

                // Create connection and tables
                using (var conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    CreateTables(conn);
                }

                // Initialize sample subjects for new database
                // Disabled to keep database empty and let user manage entries
                // if (isNewDatabase)
                // {
                //     var subjectService = new SubjectService();
                //     subjectService.InitializeSampleData();
                // }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to initialize database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Create database tables
        /// </summary>
        private static void CreateTables(SQLiteConnection conn)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Subjects(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Code TEXT NOT NULL UNIQUE,
                        Name TEXT NOT NULL,
                        Instructor TEXT,
                        Credits INTEGER DEFAULT 3,
                        Color TEXT DEFAULT '#3498db'
                    );

                    CREATE TABLE IF NOT EXISTS Tasks(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        DueDate TEXT NOT NULL,
                        Priority TEXT NOT NULL DEFAULT 'Medium',
                        IsCompleted INTEGER NOT NULL DEFAULT 0,
                        Subject TEXT,
                        SubjectName TEXT,
                        Description TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Schedule(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        DayOfWeek INTEGER NOT NULL,
                        SubjectCode TEXT NOT NULL,
                        SubjectName TEXT,
                        StartTime TEXT NOT NULL,
                        EndTime TEXT NOT NULL,
                        Location TEXT,
                        Instructor TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Todos(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        IsCompleted INTEGER NOT NULL DEFAULT 0,
                        Category TEXT DEFAULT 'Personal',
                        CreatedDate TEXT NOT NULL,
                        ModifiedDate TEXT
                    );

                    CREATE INDEX IF NOT EXISTS idx_subjects_code ON Subjects(Code);
                    CREATE INDEX IF NOT EXISTS idx_tasks_duedate ON Tasks(DueDate);
                    CREATE INDEX IF NOT EXISTS idx_schedule_day ON Schedule(DayOfWeek);
                    CREATE INDEX IF NOT EXISTS idx_todos_completed ON Todos(IsCompleted);
                ";
                cmd.ExecuteNonQuery();
                
                // Add SubjectName column to existing Tasks table if it doesn't exist
                AddSubjectNameColumn(conn);
                
                // Add ModifiedDate column to existing Todos table if it doesn't exist
                AddModifiedDateColumn(conn);
                
                // Migrate existing data to populate SubjectName
                MigrateTaskSubjectNames(conn);
            }
        }

        /// <summary>
        /// Add SubjectName column to Tasks table if it doesn't exist
        /// </summary>
        private static void AddSubjectNameColumn(SQLiteConnection conn)
        {
            try
            {
                using (var cmd = conn.CreateCommand())
                {
                    // Check if column exists
                    cmd.CommandText = "PRAGMA table_info(Tasks)";
                    using (var reader = cmd.ExecuteReader())
                    {
                        bool hasSubjectName = false;
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == "SubjectName")
                            {
                                hasSubjectName = true;
                                break;
                            }
                        }
                        
                        if (!hasSubjectName)
                        {
                            // Add the column
                            using (var alterCmd = conn.CreateCommand())
                            {
                                alterCmd.CommandText = "ALTER TABLE Tasks ADD COLUMN SubjectName TEXT";
                                alterCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Column might already exist or table might be new
            }
        }

        /// <summary>
        /// Add ModifiedDate column to Todos table if it doesn't exist
        /// </summary>
        private static void AddModifiedDateColumn(SQLiteConnection conn)
        {
            try
            {
                using (var cmd = conn.CreateCommand())
                {
                    // Check if column exists
                    cmd.CommandText = "PRAGMA table_info(Todos)";
                    using (var reader = cmd.ExecuteReader())
                    {
                        bool hasModifiedDate = false;
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == "ModifiedDate")
                            {
                                hasModifiedDate = true;
                                break;
                            }
                        }
                        
                        if (!hasModifiedDate)
                        {
                            // Add the column
                            using (var alterCmd = conn.CreateCommand())
                            {
                                alterCmd.CommandText = "ALTER TABLE Todos ADD COLUMN ModifiedDate TEXT";
                                alterCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Column might already exist or table might be new
            }
        }

        /// <summary>
        /// Migrate existing Tasks to populate SubjectName from Subjects table
        /// </summary>
        private static void MigrateTaskSubjectNames(SQLiteConnection conn)
        {
            try
            {
                using (var cmd = conn.CreateCommand())
                {
                    // Update Tasks with SubjectName from Subjects table
                    cmd.CommandText = @"
                        UPDATE Tasks 
                        SET SubjectName = (
                            SELECT Name 
                            FROM Subjects 
                            WHERE Subjects.Code = Tasks.Subject
                        )
                        WHERE Subject IS NOT NULL 
                        AND Subject != '' 
                        AND (SubjectName IS NULL OR SubjectName = '')
                    ";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                // Migration might fail if tables don't exist yet
            }
        }

        /// <summary>
        /// Get project root Data folder path
        /// </summary>
        private static string GetProjectDataPath()
        {
            // Get the directory where the executable is located
            var exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            
            // Navigate to project root (go up from bin\Debug)
            var projectRoot = System.IO.Path.GetFullPath(System.IO.Path.Combine(exeDir, "..", ".."));
            
            // Create Data folder path
            var dataPath = System.IO.Path.Combine(projectRoot, "Data", "uni.db");
            
            return $"Data Source={dataPath};Version=3;foreign keys=true;";
        }

        /// <summary>
        /// Get full database file path
        /// </summary>
        private static string GetDatabasePath()
        {
            var cs = ConnectionString;
            var match = System.Text.RegularExpressions.Regex.Match(cs, @"Data Source=([^;]+)");

            if (match.Success)
            {
                var path = match.Groups[1].Value;

                if (path.Contains("|DataDirectory|"))
                {
                    var replacements = new[]
                    {
                        AppDomain.CurrentDomain.GetData("DataDirectory") as string,
                        AppDomain.CurrentDomain.BaseDirectory,
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..")
                    };

                    foreach (var basePath in replacements)
                    {
                        if (!string.IsNullOrWhiteSpace(basePath))
                        {
                            var resolved = Path.GetFullPath(path.Replace("|DataDirectory|", basePath));
                            var dir = Path.GetDirectoryName(resolved);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            return resolved;
                        }
                    }
                }

                return path;
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "uni.db");
        }

        /// <summary>
        /// Check if database exists
        /// </summary>
        public static bool DatabaseExists()
        {
            return File.Exists(GetDatabasePath());
        }

        /// <summary>
        /// Delete database file (for testing/reset)
        /// </summary>
        public static void DeleteDatabase()
        {
            var path = GetDatabasePath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}

