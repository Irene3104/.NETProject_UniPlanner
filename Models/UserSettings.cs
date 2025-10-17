using System;
using System.IO;
using Newtonsoft.Json;

namespace UniPlanner.Models
{
    /// <summary>
    /// User settings implementation with JSON persistence
    /// </summary>
    public class UserSettings : IUserSettings
    {
        private const string SettingsFileName = "settings.json";
        
        public string UserName { get; set; }
        public string Theme { get; set; }
        public bool EnableNotifications { get; set; }
        public int NotificationMinutes { get; set; }

        public UserSettings()
        {
            // Default values
            UserName = "Student";
            Theme = "Light";
            EnableNotifications = true;
            NotificationMinutes = 30;
        }

        /// <summary>
        /// Save settings to JSON file
        /// </summary>
        public void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(SettingsFileName, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save settings", ex);
            }
        }

        /// <summary>
        /// Load settings from JSON file
        /// </summary>
        public void Load()
        {
            try
            {
                if (File.Exists(SettingsFileName))
                {
                    string json = File.ReadAllText(SettingsFileName);
                    var loaded = JsonConvert.DeserializeObject<UserSettings>(json);
                    
                    if (loaded != null)
                    {
                        UserName = loaded.UserName;
                        Theme = loaded.Theme;
                        EnableNotifications = loaded.EnableNotifications;
                        NotificationMinutes = loaded.NotificationMinutes;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load settings", ex);
            }
        }

        /// <summary>
        /// Reset to default values
        /// </summary>
        public void Reset()
        {
            UserName = "Student";
            Theme = "Light";
            EnableNotifications = true;
            NotificationMinutes = 30;
            Save();
        }
    }
}

