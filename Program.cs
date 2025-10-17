using System;
using System.Windows.Forms;
using UniPlanner.Forms;
using UniPlanner.Services;

namespace UniPlanner
{
    /// <summary>
    /// Application entry point
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main entry point for the application
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                // Enable visual styles for modern UI
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Configure data directory to project root Data folder
                var projectDataDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Data"));
                if (!System.IO.Directory.Exists(projectDataDir))
                {
                    System.IO.Directory.CreateDirectory(projectDataDir);
                }
                AppDomain.CurrentDomain.SetData("DataDirectory", projectDataDir);

                // Initialize database
                DbBootstrap.EnsureCreated();

                // Run main form
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                string errorMessage = $"Application startup error:\n{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nDetails: {ex.InnerException.Message}";
                }
                errorMessage += "\n\nPlease contact support.";

                MessageBox.Show(
                    errorMessage,
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
