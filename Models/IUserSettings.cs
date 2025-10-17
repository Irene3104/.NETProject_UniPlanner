namespace UniPlanner.Models
{
    /// <summary>
    /// Interface for user settings management
    /// </summary>
    public interface IUserSettings
    {
        string UserName { get; set; }
        string Theme { get; set; }
        bool EnableNotifications { get; set; }
        int NotificationMinutes { get; set; }
        
        void Save();
        void Load();
        void Reset();
    }
}

